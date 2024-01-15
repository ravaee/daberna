using Daberna.Domain;
using Daberna.Events;
using Daberna.Services.Interfaces;

namespace Daberna.Services;

public class GameService : IGameService
{
    private readonly UserInfoAccessor _userInfoAccessor;
    private readonly GeneralEvents _generalEvents;
    private static List<Game> CurrentGames { get; } = new();

    public GameService(
        UserInfoAccessor userInfoAccessor,
        GeneralEvents generalEvents)
    {
        _userInfoAccessor = userInfoAccessor;
        _generalEvents = generalEvents;
    }

    public async Task Start(Guid gameId)
    {
        await CurrentGames.First(g => g.Id == gameId).Start();
    }

    public async Task<Game> CreateGame()
    {
        Game game = new Game(_generalEvents)
        {
            Id = Guid.NewGuid(),
            Owner = new Player
            {
                Name = (await _userInfoAccessor.GetCurrentUser()).Email,
                Id = (await _userInfoAccessor.GetCurrentUser()).Id
            }
        };

        CurrentGames.Add(game);

        _generalEvents.OnGameAdded(game.Id);

        return await Task.FromResult(game);
    }

    public async Task<List<Game>> GetAll()
    {
        return await Task.FromResult(CurrentGames.ToList());
    }

    public Task AssignPlayerToGame(Player player, Guid gameId)
    {
        Game game = CurrentGames.First(g => g.Id == gameId);

        game.Players.Add(player);

        _generalEvents.OnPlayerAddedToGame(gameId, player.Id);

        return Task.CompletedTask;
    }

    public Task RemovePlayerFromGame(string playerId, Guid gameId)
    {
        var game = CurrentGames.FirstOrDefault(g => g.Id == gameId);
        
        if (game is null)
            return Task.CompletedTask;

        game.Players.Remove(game.Players.First(p => p.Id == playerId));

        _generalEvents.OnPlayerRemovedFromGame(gameId, playerId);
        
        return Task.CompletedTask;
    }

    public Task RemoveGame(Guid gameId)
    {
        var game = CurrentGames.FirstOrDefault(g => g.Id == gameId);

        if (game is null)
            return Task.CompletedTask;
        
        CurrentGames.Remove(game);

        _generalEvents.OnGameRemoved(gameId);

        return Task.CompletedTask;
    }

    public async Task<Game> GetGame(Guid id)
    {
        var game = CurrentGames.First(g => g.Id == id);

        return await Task.FromResult(game);
    }
}