using System.Net.Sockets;

namespace Daberna.Test.Client;

class TcpClientApp
{
    private TcpClient client;
    private NetworkStream stream;

    public void ConnectToServer(string ipAddress, int port)
    {
        client = new TcpClient();
        client.BeginConnect(ipAddress, port, new AsyncCallback(OnConnected), null);
    }

    private void OnConnected(IAsyncResult ar)
    {
        client.EndConnect(ar);
        Console.WriteLine("Connected to server.");
        stream = client.GetStream();
    }
}