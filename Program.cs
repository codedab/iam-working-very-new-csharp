using IdentityApi.Services;

var builder = WebApplication.CreateBuilder(args);

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
