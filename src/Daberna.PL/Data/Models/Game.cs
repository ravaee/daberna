namespace Daberna.Data.Models;
public class Game
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreateDate { get; set; }
    public decimal EntryCost { get; set; }
    public bool IsPrivate { get; set; }
    public int Keycode { get; set; }
    public List<User>? Players { get; set; }
    public Status Status { get; set; }
}

public enum Status
{
    Created,
    Started,
    Finished
}