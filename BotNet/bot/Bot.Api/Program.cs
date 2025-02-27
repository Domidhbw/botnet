using Bot.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddScoped<ICommandService, CommandService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<ICallCCS, CallCCS>();
builder.Services.AddCors(options => options.AddPolicy(name: "AllowAngularApp",
    policy =>
    {
        policy.WithOrigins("https://localhost:5003", "http://localhost:5003", "https://localhost:5002", "http://localhost:5002").AllowAnyMethod().AllowAnyHeader();
    }));
var app = builder.Build();
app.UseCors("AllowAngularApp");
app.MapControllers();
app.UseRouting();


app.Lifetime.ApplicationStarted.Register(() =>
{ 
    Task.Run(async () =>
    {
        using (var scope = app.Services.CreateScope())
        {
            var timerPostService = scope.ServiceProvider.GetRequiredService<ICallCCS>();
            await timerPostService.WaitAndPostToApi();
        }
    });
});

app.Run();


