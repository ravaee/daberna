using Daberna.Services.Interfaces;
using Daberna.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Daberna.PL.Pages;

public class IndexPage : ComponentBase
{
    [Inject] 
    IGameService GameService { get; set; } = null!;
    
    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    protected IndexViewModel Model {get; set;} = new();

    protected async Task OnCreateGameClicked()
    {
        Domain.Game game = await GameService.CreateGame();
        GoToGameRoom(game.Id);
    }

    protected override async Task OnInitializedAsync()
    {
        Model.Games = await GameService.GetAll();
    }

    protected async Task OnEnterGameRoomClicked(Guid gameId)
    {
        GoToGameRoom(gameId);
    }

    private void GoToGameRoom(Guid gameId)
    {
        NavigationManager.NavigateTo($"/game/{gameId}");
    }
}
