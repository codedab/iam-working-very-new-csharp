using IdentityApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Read port from ASPNETCORE_URLS env var; default to 8080
var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://+:8080";
builder.WebHost.UseUrls(urls);

builder.Services.AddSingleton<UserStore>();
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<RoleStore>();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseMiddleware<JwtMiddleware>();
app.MapHealthChecks("/health");
app.MapControllers();

app.Run();
