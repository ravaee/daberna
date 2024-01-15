using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;

var server = new ChatServerCustom();
server.Start();


class ChatServer
{
    private TcpListener listener;
    private ConcurrentDictionary<string, TcpClient> clients = new();
    

    public ChatServer()
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
            string name = reader.ReadLine();
            clients.TryAdd(name, client);

            Console.WriteLine($"{name} connected.");

            string input;
            while ((input = reader.ReadLine()) != null)
            {
                var parts = input.Split('|');
                var command = parts[0];
                var message = parts[1];

                switch (command)
                {
                    case "private":
                        SendPrivateMessage(parts[2], $"{name}: {message}");
                        break;
                    case "broadcast":
                        BroadcastMessage($"{name}: {message}");
                        break;
                    case "group":
                        SendGroupMessage(parts[2], $"{name}: {message}");
                        break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception: {0}", e.Message);
        }
    }

    private void SendPrivateMessage(string targetName, string message)
    {
        if (clients.TryGetValue(targetName, out TcpClient targetClient))
        {
            StreamWriter writer = new StreamWriter(targetClient.GetStream()) { AutoFlush = true };
            writer.WriteLine(message);
        }
    }

    private void BroadcastMessage(string message)
    {
        foreach (var client in clients.Values)
        {
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            writer.WriteLine(message);
        }
    }

    private void SendGroupMessage(string groupName, string message)
    {
        var groupMembers = groupName.Split(',');
        foreach (var member in groupMembers)
        {
            if (clients.TryGetValue(member, out TcpClient targetClient))
            {
                StreamWriter writer = new StreamWriter(targetClient.GetStream()) { AutoFlush = true };
                writer.WriteLine(message);
            }
        }
    }
}