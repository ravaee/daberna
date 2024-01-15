using Daberna.Domain;
using Daberna.Events;
using Daberna.Shared;
using Daberna.ViewModels;

namespace Daberna.Pages;

public class IndexPage : BaseAuthPage
{
    protected IndexViewModel Model { get; } = new();

    protected async Task OnCreateGameClicked()
    {
        Domain.Game game = await GameService.CreateGame();
        
        Model.Games = await GameService.GetAll();
        await InvokeAsync(StateHasChanged); 
        await GoToGameRoom(game.Id);
    }

    protected override async Task OnInitializedAsync()
    {
        Events.GameAdded += async (gameId) =>
        {
            Model.Games = await GameService.GetAll();
            await InvokeAsync(StateHasChanged);
        };
        
        Events.GameRemoved += async (gameId) =>
        {
            Model.Games = await GameService.GetAll();
            await InvokeAsync(StateHasChanged);
        };
        
        Model.Games = await GameService.GetAll();
    }

    protected async Task OnEnterGameRoomClicked(Guid gameId)
    {
        await GoToGameRoom(gameId);
    }

    private async Task GoToGameRoom(Guid gameId)
    {
        await GameService.AssignPlayerToGame(new Player()
        {
            Id = (await UserInfoAccessor.GetCurrentUser()).Id,
            Name = (await UserInfoAccessor.GetCurrentUser()).Email,
        }, gameId);
        
        NavigationManager.NavigateTo($"/game/{gameId}");
    }
}
