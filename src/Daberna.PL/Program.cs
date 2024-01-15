using Daberna.Services;
using Daberna.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
  googleOptions.ClientId = "1060825257321-9p0k9qqu9432m6aor0vijp6nqlca6h51.apps.googleusercontent.com";//builder.Configuration["Authentication:Google:ClientId"];
  googleOptions.ClientSecret = "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"; //builder.Configuration["Authentication:Google:ClientSecret"];
});

 
builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<IGameService, GameService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

try
{
    app.Run();
}
catch (Exception ex)
{
    // Log the exception or take any desired action.
    // Note: In a production environment, you should use a robust logging framework.
    Console.WriteLine($"An error occurred: {ex.Message}");
}
