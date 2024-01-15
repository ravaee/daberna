namespace Daberna.Data.Models;

public class User
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime FirstLogin { get; set; }
    public DateTime LastLogin { get; set; }
    public string? Username { get; set; }
    public string? Fullname { get; set; }
    public string? Nickname { get; set; }
    public long Gem { get; set; }
    public decimal Money { get; set; } 
}
