using System.Net.Sockets;

namespace Daberna.Common;

public class SocketSettings
{
    public const string ServerPort = "5000";
    public const string ServerIP = "localhost";
}

public record SharedObject
{
    public string Message { get; init; }
    public MessageType MessageType { get; init; }
    public Guid DestinationPlayerId { get; set; } // this is for private message
    public Guid GameId { get; set; } //this is same as group now ignore it I will describe it to you in future
}

public enum MessageType
{
    CreateGame,
    JoinGame,
    PrivateMessage,
    BroadCastMessage,
    GameMessage
}

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid GameId { get; set; }
    public TcpClient client { get; set; }
}

public class Game
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid GameId { get; set; }
    public TcpClient client { get; set; }
    public GameStatus Status { get; set; }

    public void Start()
    {
        
    }
}

public enum GameStatus
{
    Created,
    Started,
    Finished
}