using Daberna.Shared;
using Microsoft.AspNetCore.Components;

namespace Daberna.Pages;

public class GamePage : BaseAuthPage, IDisposable
{
    [Parameter]
    public string? GameId { get; set; }
    protected Domain.Game? Game { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        Game = await GameService.GetGame(Guid.Parse(GameId));

        Events.StoneChanged += () =>
        {
            InvokeAsync(StateHasChanged).Wait(); 
        };

        Events.PlayerAddedToGame += (GameId, playerId) =>
        {
            InvokeAsync(StateHasChanged).Wait();
        };
    }

    protected async Task OnStartGameClicked(Guid gameId)
    {
        await GameService.Start(gameId);
    }
    
    public void Dispose()
    {
        var user = UserInfoAccessor.GetCurrentUser().Result;

        if (Game?.Owner.Id == user.Id)
        {
            GameService.RemoveGame(Game.Id).Wait();
        }
        else
        {
            GameService.RemovePlayerFromGame(user.Id, Game?.Id ?? throw new AggregateException());
        }
    }
}
