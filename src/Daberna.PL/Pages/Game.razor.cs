using Daberna.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Daberna.PL.Pages;

public class GamePage : ComponentBase
{
    public GamePage()
    {
        
    }
    
    [Inject] 
    private IGameService GameService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Parameter]
    public string GameId { get; set; }

    protected Domain.Game? Game { get; set; }


    protected override async Task OnInitializedAsync()
    {
        if (string.IsNullOrEmpty(GameId))
        {
            return;
        }

        Game = await GameService.GetGame(Guid.Parse(GameId));
    }

}
