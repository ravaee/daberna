using Daberna.Domain;

namespace Daberna.Services.Interfaces;

public interface IGameService
{
    Task<Game> CreateGame();
    Task<Game> GetGame(Guid id);
    Task<List<Game>> GetAll();
}
