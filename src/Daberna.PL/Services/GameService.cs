using Daberna.Domain;
using Daberna.Services.Interfaces;

namespace Daberna.Services;

public class GameService : IGameService
{
    private static List<Game> CurrentGames { get; set;} = new();
 
    public async Task<Game> CreateGame()
    {
        Game game = new Game()
        {
            Id = Guid.NewGuid(),
        };

        CurrentGames.Add(game);

        return await Task.FromResult(game);
    }

    public async Task<List<Game>> GetAll()
    {
        return await Task.FromResult(CurrentGames);
    }

    public async Task<Game> GetGame(Guid id)
    {
        var game = CurrentGames.Where(g => g.Id == id).FirstOrDefault();

        return await Task.FromResult(game);
    }

    
}