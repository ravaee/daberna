namespace Daberna.Domain;

public partial class Game
{
    public Guid Id { get; set; }
    public Player Owner { get; init; } = null!;
    public List<Player> Players { get; } = new();
    private List<Stone> Stones { get; } = new()
    {
        new() { Number = 1 },
        new() { Number = 2 },
        new() { Number = 3 },
        new() { Number = 4 },
        new() { Number = 5 },
        new() { Number = 6 },
        new() { Number = 7 },
        new() { Number = 8 },
        new() { Number = 9 },
        new() { Number = 10 },
        new() { Number = 11 },
        new() { Number = 12 },
        new() { Number = 13 },
        new() { Number = 14 },
        new() { Number = 15 },
        new() { Number = 16 },
        new() { Number = 17 },
        new() { Number = 18 },
        new() { Number = 19 },
        new() { Number = 20 },
        new() { Number = 21 },
        new() { Number = 22 },
        new() { Number = 23 },
        new() { Number = 24 },
        new() { Number = 25 },
        new() { Number = 26 },
        new() { Number = 27 },
        new() { Number = 28 },
        new() { Number = 29 },
        new() { Number = 30 },
        new() { Number = 31 },
        new() { Number = 32 },
        new() { Number = 33 },
        new() { Number = 34 },
        new() { Number = 35 },
        new() { Number = 36 },
        new() { Number = 37 },
        new() { Number = 38 },
        new() { Number = 39 },
        new() { Number = 40 },
        new() { Number = 41 },
        new() { Number = 42 },
        new() { Number = 43 },
        new() { Number = 44 },
        new() { Number = 45 },
        new() { Number = 46 },
        new() { Number = 47 },
        new() { Number = 48 },
        new() { Number = 49 },
        new() { Number = 50 },
        new() { Number = 51 },
        new() { Number = 52 },
        new() { Number = 53 },
        new() { Number = 54 },
        new() { Number = 55 },
        new() { Number = 56 },
        new() { Number = 57 },
        new() { Number = 58 },
        new() { Number = 59 },
        new() { Number = 60 },
        new() { Number = 61 },
        new() { Number = 62 },
        new() { Number = 63 },
        new() { Number = 64 },
        new() { Number = 65 },
        new() { Number = 66 },
        new() { Number = 67 },
        new() { Number = 68 },
        new() { Number = 69 },
        new() { Number = 70 },
        new() { Number = 71 },
        new() { Number = 72 },
        new() { Number = 73 },
        new() { Number = 74 },
        new() { Number = 75 },
        new() { Number = 76 },
        new() { Number = 77 },
        new() { Number = 78 },
        new() { Number = 79 },
        new() { Number = 80 },
        new() { Number = 81 },
        new() { Number = 82 },
        new() { Number = 83 },
        new() { Number = 84 },
        new() { Number = 85 },
        new() { Number = 86 },
        new() { Number = 87 },
        new() { Number = 88 },
        new() { Number = 89 },
        new() { Number = 90 },
    };
    public Queue<Stone> CurrentStones { get; } = new();
}