namespace Daberna.Common;

public class MessageContract
{
    public Guid SenderId { get; set; }
}

public class CreateGame : MessageContract
{
    public string? GameName { get; set; }
}

public class GetAllGames : MessageContract
{
    public List<Game>? Games { get; set; }
}

public class PrivateMessage : MessageContract
{
    public Guid ReciverId { get; set; }
    public string? Message { get; set; }

} 

public class BroadcastMessage : MessageContract
{
    public string? Message { get; set; }
}


