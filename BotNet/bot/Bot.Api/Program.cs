using Bot.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<ICommandService, CommandService>();

var app = builder.Build();

app.MapControllers();

app.Run();


