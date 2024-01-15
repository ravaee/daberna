using Daberna.Events;

namespace Daberna.Domain;

public partial class Game
{
    private readonly GeneralEvents _events;
    private readonly PeriodicTimer _periodicTimer = new(TimeSpan.FromSeconds(5));

    public Game(GeneralEvents events)
    {
        _events = events;
    }
    public async Task Start()
    {
        while (await _periodicTimer.WaitForNextTickAsync())
        {
            List<Stone> notMarkedStones = Stones.Where(stone => !stone.Marked).ToList();
            Stone stone = notMarkedStones[new Random().Next(notMarkedStones.Count - 1)];
            
            stone.Marked = true;

            if (CurrentStones.Count >= 5)
            {
                CurrentStones.Dequeue();
            }
            
            CurrentStones.Enqueue(stone);
            
            _events.OnStoneChanged();
        }
    }
}


