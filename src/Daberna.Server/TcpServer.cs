using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Daberna.Common;
using Newtonsoft.Json;

class ChatServerCustom
{
    private TcpListener listener;
    private ConcurrentDictionary<Guid, Player> Players = new();
    private ConcurrentDictionary<Guid, Game> Games = new();

    public ChatServerCustom()
    {
        listener = new TcpListener(IPAddress.Any, 5000);
    }

    public void Start()
    {
        listener.Start();
        Console.WriteLine("Server started on port 5000.");

        try
        {
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(HandleClient, client);
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine("SocketException: {0}", e);
            listener.Stop();
        }
    }

    private void HandleClient(object obj)
    {
        TcpClient client = (TcpClient)obj;
        var stream = client.GetStream();
        StreamReader reader = new StreamReader(stream);
        StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

        try
        {
            string playerName = reader.ReadLine();
            var playerId = Guid.NewGuid();
            var player = new Player
            {
                Id = playerId,
                Name = playerName,
                client = client
            };

            Players.TryAdd(player.Id, player);
            Console.WriteLine($"{player.Name} with Id: {player.Id} connected.");

            writer.WriteLine(playerId.ToString());

            string input;

            while ((input = reader.ReadLine()) != null)
            {
                var messageObject = JsonConvert.DeserializeObject<MessageObject>(input, new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });

                switch (messageObject.MessageType)
                {
                    case MessageType.PrivateMessage:
                        PrivateMessage privateMessage = (PrivateMessage)messageObject.MessageContract;
                        SendPrivateMessage(privateMessage);
                        break;
                    case MessageType.BroadCastMessage:
                        BroadcastMessage broadcastMessage = (BroadcastMessage)messageObject.MessageContract;
                        BroadcastMessage(broadcastMessage);
                        break;
                    case MessageType.CreateGame:
                        CreateGame createGame = (CreateGame)messageObject.MessageContract;
                        CreateNewGame(createGame);
                        break;
                    case MessageType.GetGames:
                        GetAllGames getAllGames = (GetAllGames)messageObject.MessageContract;
                        SendAllGamesToPlayer(getAllGames);
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
        }
    }

    private void SendAllGamesToPlayer(GetAllGames getAllGames)
    {
        var messageObject = new MessageObject
        {
            MessageType = MessageType.GetGames,
            MessageStatus = MessageStatus.Done,
            MessageContract = new GetAllGames
            {
                Games = Games.Values.ToList(),
                SenderId = Guid.Empty
            }
        };

        var settings = new JsonSerializerSettings();

        settings.Converters.Add(new IPAddressConverter());
        settings.TypeNameHandling = TypeNameHandling.All;

        string json = JsonConvert.SerializeObject(messageObject, settings);

        if (Players.TryGetValue(getAllGames.SenderId, out Player player))
        {
            StreamWriter writer = new StreamWriter(player.client.GetStream()) { AutoFlush = true };

            writer.WriteLine(json);
        }
    }

    private void SendAllGamesToAllPlayers()
    {
        MessageObject messageObject = new MessageObject
        {
            MessageType = MessageType.GetGames,
            MessageStatus = MessageStatus.Done,
            MessageContract = new GetAllGames
            {
                Games = Games.Values.ToList(),
                SenderId = Guid.Empty
            }
        };

        var settings = new JsonSerializerSettings();

        settings.Converters.Add(new IPAddressConverter());
        settings.TypeNameHandling = TypeNameHandling.All;

        string json = JsonConvert.SerializeObject(messageObject, settings);

        foreach (var player in Players.Values)
        {
            StreamWriter writer = new StreamWriter(player.client.GetStream()) { AutoFlush = true };

            writer.WriteLine(json);
        }
    }

    private void CreateNewGame(CreateGame createGame)
    {
        try
        {
            Players.TryGetValue(createGame.SenderId, out var player);

            if (player is null)
                return;

            Game game = new Game
            {
                Id = Guid.NewGuid(),
                Name = createGame.GameName ?? "GameName",
                Status = GameStatus.Created,
                Players = new List<Player>
                {
                    player
                }
            };

            Games.TryAdd(game.Id, game);

            SendAllGamesToAllPlayers();
        }
        catch (Exception e)
        {
            SendPrivateMessage(new PrivateMessage
            {
                SenderId = Guid.Empty,
                Message = e.Message,
                ReciverId = createGame.SenderId
            });
        }
    }

    private void SendPrivateMessage(PrivateMessage privateMessage)
    {
        SendMessageToPlayer(
            privateMessage.ReciverId,
            privateMessage,
            MessageType.PrivateMessage,
            MessageStatus.Done);
    }

    private void BroadcastMessage(BroadcastMessage broadcastMessage)
    {
        SendMessageToAll(
            broadcastMessage,
            MessageType.BroadCastMessage,
            MessageStatus.Done);
    }

    private void SendMessageToPlayer(
        Guid playerId,
        MessageContract messageContract,
        MessageType messageType,
        MessageStatus messageStatus)
    {
        try
        {
            var messageObject = new MessageObject
            {
                MessageType = messageType,
                MessageStatus = messageStatus,
                MessageContract = messageContract
            };

            string json = JsonConvert.SerializeObject(messageObject, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            if (Players.TryGetValue(playerId, out Player player))
            {
                StreamWriter writer = new StreamWriter(player.client.GetStream()) { AutoFlush = true };

                writer.WriteLine(json);
            }
            else
            {
                throw new Exception("No player exist");
            }
        }
        catch (Exception e)
        {
            throw new Exception("Could not send message to Player", e);
        }
    }

    private void SendMessageToAll(
        MessageContract messageContract,
        MessageType messageType,
        MessageStatus messageStatus)
    {
        try
        {
            var messageObject = new MessageObject
            {
                MessageType = messageType,
                MessageStatus = messageStatus,
                MessageContract = messageContract
            };

            string json = JsonConvert.SerializeObject(messageObject, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            foreach (var player in Players.Values)
            {
                StreamWriter writer = new StreamWriter(player.client.GetStream()) { AutoFlush = true };

                writer.WriteLine(json);
            }
        }
        catch (Exception e)
        {
            throw new Exception("Could not send message to Player", e);
        }
    }
}
