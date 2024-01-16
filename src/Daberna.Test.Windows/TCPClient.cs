using Daberna.Common;
using Newtonsoft.Json;
using System.IO;
using System.Net.Sockets;

namespace Daberna.Test.Windows;

public class TCPClient
{

    private TcpClient _tcpClient;
    private StreamReader _reader;
    private StreamWriter _writer;
    private Guid _playerId;
    private object _lockObject = new object();
    private bool _isConnected;
    private NetworkStream _stream;

    public event Action Connected;
    public event Action<List<Game>> GamesHasTaken;
    public event Action GameHasCreated;

    public TCPClient(string serverAddress, int port)
    {
        _tcpClient = new TcpClient(serverAddress, port);
        _stream = _tcpClient.GetStream();
        _reader = new StreamReader(_stream);
        _writer = new StreamWriter(_stream) { AutoFlush = true };
        _isConnected = true;
    }

    public void EnterNameToConnect(string name)
    {
        _writer.WriteLine(name);
        _playerId = Guid.Parse(_reader.ReadLine());

        Start();

        Connected();
    }


    public void Start()
    {
        var thread = new Thread(ListenToServer);
        thread.Start();
    }

    private void ListenToServer()
    {
        try
        {
            string input;

            while (_isConnected && (input = _reader.ReadLine()) != null)
            {
                var settings = new JsonSerializerSettings();

                settings.Converters.Add(new IPAddressConverter());
                settings.TypeNameHandling = TypeNameHandling.All;

                var messageObject = JsonConvert.DeserializeObject<MessageObject>(input, settings);

                if (messageObject?.MessageType == MessageType.GetGames)
                {
                    GetAllGames getAllGames = (GetAllGames) messageObject.MessageContract;

                    GamesHasTaken(getAllGames?.Games ?? new List<Game>());
                }

                if(messageObject?.MessageType == MessageType.CreateGame)
                {
                    CreateGame createGame = (CreateGame)messageObject.MessageContract;

                    GameHasCreated();
                }                
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception: {e.Message}");
        }
        finally
        {
            _tcpClient.Close();
        }
    }

    public void SendGetAllGamesMessage()
    {
        var messageObject = new MessageObject
        {
            MessageType = MessageType.GetGames,
            MessageContract = new GetAllGames
            {
                SenderId = _playerId
            },
            MessageStatus = MessageStatus.Pending
        };

        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new IPAddressConverter());
        settings.TypeNameHandling = TypeNameHandling.All;

        string json = JsonConvert.SerializeObject(messageObject, settings);

        SafeWriteLine(json);
    }

    public void SendCreateGameMessage(string name)
    {
        var messageObject = new MessageObject
        {
            MessageType = MessageType.CreateGame,
            MessageContract = new CreateGame
            {
                SenderId = _playerId,
                GameName = name
            },
        };

        var settings = new JsonSerializerSettings();
        settings.Converters.Add(new IPAddressConverter());
        settings.TypeNameHandling = TypeNameHandling.All;

        string json = JsonConvert.SerializeObject(messageObject, settings);

        SafeWriteLine(json);
    }

    private void SafeWriteLine(string message)
    {
        lock (_lockObject)
        {
            if (_isConnected && _tcpClient.Connected)
            {
                try
                {
                    _writer.WriteLine(message);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Write Exception: {e.Message}");
                    CloseConnection();
                }
            }
        }
    }

    private void CloseConnection()
    {
        if (_isConnected)
        {
            _isConnected = false;
            _tcpClient?.Close();
            _stream?.Dispose();
            _reader?.Dispose();
            _writer?.Dispose();
        }
    }
}
