// using Daberna.Services.Interfaces;
// using Daberna.ViewModels;
// using Microsoft.AspNetCore.Components;
// using Daberna.Domain;
// using Microsoft.AspNetCore.Components.Authorization;
//
// namespace Daberna.Pages;
//
// public class BaseAuthPage : ComponentBase
// {
//     [Inject] private AuthenticationStateProvider AuthenticationStateProvider { get; set; }
//
//     public Player GetCurrentUser(){
//         var user = AuthenticationStateProvider.GetAuthenticationStateAsync().Result.User;
//         
//         return new Player()
//         {
//             Name = user.Identity?.Name,
//             Id = user.FindFirst(c => c.Type == "sub")?.Value
//         };
//     }
// }