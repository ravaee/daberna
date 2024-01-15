using Daberna.Domain;

namespace Daberna.Services.Interfaces;

public interface IGameService
{
    Task Start(Guid gameId);
    Task<Game> CreateGame();
    Task<Game> GetGame(Guid id);
    Task<List<Game>> GetAll();
    Task AssignPlayerToGame(Player player, Guid gameId);
    Task RemoveGame(Guid gameId);
    Task RemovePlayerFromGame(string playerId, Guid gameId);
}
