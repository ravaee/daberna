// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Daberna.Areas.Identity.Pages.Account;

[AllowAnonymous]
public class ExternalLoginModel : PageModel
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly ILogger<ExternalLoginModel> _logger;

    public ExternalLoginModel(
        SignInManager<IdentityUser> signInManager,
        UserManager<IdentityUser> userManager,
        IUserStore<IdentityUser> userStore,
        ILogger<ExternalLoginModel> logger,
        IEmailSender emailSender)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = (IUserEmailStore<IdentityUser>)_userStore;
        _logger = logger;
    }


    public IActionResult OnGet() => RedirectToPage("./Login");

    public IActionResult OnPost(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Page("./ExternalLogin", pageHandler: "Callback", values: new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
        returnUrl ??= Url.Content("~/");

        if (remoteError != null)
        {
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        var info = await _signInManager.GetExternalLoginInfoAsync();

        if (info is null)
        {
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        var signInResult = await TrySignInIfUserExist(info);
            
        if (signInResult)
        {
            return LocalRedirect(returnUrl);
        }
            
        var user = await CreateUser(info);

        if (user is null)
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });

        signInResult = await TrySignInByUserData(user, info);
            
        if (signInResult)
        {
            return LocalRedirect(returnUrl);
        }
            
        return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
    }

    private async Task<bool> TrySignInByUserData(IdentityUser user, ExternalLoginInfo info)
    {
        var loginResult = await _userManager.AddLoginAsync(user, info);

        if (!loginResult.Succeeded)
            return false;

        await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);

        return true;
    }

    private async Task<bool> TrySignInIfUserExist(ExternalLoginInfo info)
    {
        var result = await _signInManager.ExternalLoginSignInAsync(
            info.LoginProvider,
            info.ProviderKey,
            isPersistent: false,
            bypassTwoFactor: true);

        if (result.Succeeded)
        {
            return true;
        }

        return false;
    }

    private async Task<IdentityUser> CreateUser(ExternalLoginInfo info)
    {
        var userEmail = info.Principal.FindFirstValue(ClaimTypes.Email);

        var user = new IdentityUser();

        await _userStore.SetUserNameAsync(user, userEmail, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, userEmail, CancellationToken.None);
        await _emailStore.SetEmailConfirmedAsync(user, true, CancellationToken.None);

        var userManagerResult = await _userManager.CreateAsync(user);

        return userManagerResult.Succeeded ? user : null;
    }
}