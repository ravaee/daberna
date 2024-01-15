using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using Daberna.Common;
using Newtonsoft.Json;

class ChatServerCustom
{
    private TcpListener listener;
    private ConcurrentDictionary<Guid, Player> players = new();
    private ConcurrentDictionary<Guid, Game> games = new();

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

            players.TryAdd(player.Id, player);
            Console.WriteLine($"{player.Name} with Id: {player.Id} connected.");

            // Send the playerId back to the client
            writer.WriteLine(playerId.ToString());

            string input;
            
            while ((input = reader.ReadLine()) != null)
            {
                var sharedObject = JsonConvert.DeserializeObject<SharedObject>(input);

                switch (sharedObject.MessageType)
                {
                    case MessageType.PrivateMessage:
                        SendPrivateMessage(sharedObject.DestinationPlayerId, sharedObject.Message);
                        break;
                    case MessageType.BroadCastMessage:
                        BroadcastMessage(sharedObject.Message);
                        break;
                    case MessageType.CreateGame:
                        CreateNewGame(sharedObject);
                        break;
                    // ... handle other cases ...
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
        }
    }

    private void CreateNewGame(SharedObject sharedObject)
    {
        Game game = new Game();

        games.TryAdd(game.Id, game);
    }

    private void SendPrivateMessage(Guid id, string message)
    {
        if (players.TryGetValue(id, out Player player))
        {
            StreamWriter writer = new StreamWriter(player.client.GetStream()) { AutoFlush = true };
            writer.WriteLine(message);
        }
    }
    private void BroadcastMessage(string message)
    {
        foreach (var player in players.Values)
        {
            StreamWriter writer = new StreamWriter(player.client.GetStream()) { AutoFlush = true };
            writer.WriteLine(message);
        }
    }
    private void SendGroupMessage(string players, string message)
    {
        var groupMembers = players.Split(',');
        foreach (var member in groupMembers)
        {
            if (this.players.TryGetValue(Guid.Parse(member), out Player? player))
            {
                StreamWriter writer = new StreamWriter(player.client.GetStream()) { AutoFlush = true };
                writer.WriteLine(message);
            }
        }
    }
}
