namespace Daberna.Events;

public class GeneralEvents
{
    public event Action<Guid> GameAdded;
    public event Action<Guid> GameRemoved;
    public event Action<Guid, string> PlayerAddedToGame;
    public event Action<Guid, string> PlayerRemovedFromGame;
    public event Action StoneChanged;

    public void OnGameAdded(Guid gameId)
    {
        GameAdded?.Invoke(gameId);
    }

    public void OnGameRemoved(Guid gameId)
    {
        GameRemoved?.Invoke(gameId);
    }

    public void OnPlayerAddedToGame(Guid gameId, string playerId)
    {
        PlayerAddedToGame?.Invoke(gameId, playerId);
    }

    public void OnPlayerRemovedFromGame(Guid gameId, string playerId)
    {
        PlayerRemovedFromGame?.Invoke(gameId, playerId);
    }

    public void OnStoneChanged()
    {
        StoneChanged?.Invoke();
    }
}