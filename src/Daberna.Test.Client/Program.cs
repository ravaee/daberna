using System.Net.Sockets;
using Daberna.Common;
using Newtonsoft.Json;

var client = new ChatClient(SocketSettings.ServerIP, int.Parse(SocketSettings.ServerPort));
client.Start();

class ChatClient
{
    private TcpClient _tcpClient;
    private StreamReader _reader;
    private StreamWriter _writer;
    private Guid _playerId;

    public ChatClient(string serverAddress, int port)
    {
        _tcpClient = new TcpClient(serverAddress, port);
        NetworkStream stream = _tcpClient.GetStream();
        _reader = new StreamReader(stream);
        _writer = new StreamWriter(stream) { AutoFlush = true };
    }

    public void Start()
    {
        Console.Write("Enter your name: ");
        string? name = Console.ReadLine();
        _writer.WriteLine(name);
        
        _playerId = Guid.Parse(_reader.ReadLine());
        Console.WriteLine($"Connected to server. Your player ID is {_playerId}.");
        
        var thread = new Thread(ListenToServer);
        thread.Start();
        
        while (true)
        {
            Console.WriteLine("Enter your message: ");
            string message = Console.ReadLine();
            
            var sharedObject = new SharedObject
            {
                Message = message,
                MessageType = MessageType.BroadCastMessage,
                DestinationPlayerId = Guid.Empty, 
            };
            
            string json = JsonConvert.SerializeObject(sharedObject);
            _writer.WriteLine(json);
        }
    }

    private void ListenToServer()
    {
        try
        {
            string message;
            while ((message = _reader.ReadLine()) != null)
            {
                Console.WriteLine($"Message from server: {message}");
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
}