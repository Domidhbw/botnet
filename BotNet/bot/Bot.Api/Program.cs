using Bot.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddCors(options => options.AddPolicy(name: "AllowAngularApp",
    policy =>
    {
        policy.WithOrigins("https://localhost:5003", "http://localhost:5003").AllowAnyMethod().AllowAnyHeader();
    }));
var app = builder.Build();
app.UseCors("AllowAngularApp");
app.MapControllers();

app.Run();


