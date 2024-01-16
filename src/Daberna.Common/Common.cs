using System.Net;
using System.Net.Sockets;
using Newtonsoft.Json;

namespace Daberna.Common;

public class SocketSettings
{
    public const string ServerPort = "5000";
    public const string ServerIP = "localhost";
}

public class MessageObject
{
    public MessageType MessageType { get; set; }
    public MessageContract MessageContract { get; set; } = null!;
    public MessageStatus MessageStatus { get; set; } = MessageStatus.Pending;
}

public enum MessageType
{
    GetGames,
    CreateGame,
    JoinGame,
    PrivateMessage,
    BroadCastMessage,
    GameMessage,
    PutStoneMessage,
    NewStoneMessage,
}

public class Player
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid GameId { get; set; }

    [JsonIgnore]
    public TcpClient client { get; set; }
}

public class Game
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Player> Players { get; set; } = new();
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


public enum MessageStatus
{
    Pending,
    Done,
    Failed
}

public class IPAddressConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(IPAddress);
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        var ipAddress = value as IPAddress;
        if (ipAddress != null)
        {
            writer.WriteValue(ipAddress.ToString());
        }
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.String)
        {
            return IPAddress.Parse((string)reader.Value);
        }
        return null;
    }
}