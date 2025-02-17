using Bot.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddScoped<IFileService, FileService>();

var app = builder.Build();

app.MapControllers();

app.Run();


