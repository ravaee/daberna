
namespace Daberna.Domain;


public partial class Game
{
    public Guid Id { get; set; }
    public List<Player>? Players { get; set; }
    public List<Stone>? Stones { get; set; } = new List<Stone>()
    {
        new() { Number = 1 },
        new() { Number = 2 },
        new() { Number = 3 },
    };

}

