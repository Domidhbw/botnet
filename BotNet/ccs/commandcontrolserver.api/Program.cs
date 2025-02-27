using CommandControlServer.Api;
using CommandControlServer.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connectionString));

builder.Services.AddControllers();

builder.Services.AddCors(options => options.AddPolicy(name: "AllowLocalhost5003",
               policy =>
               {
                   policy.WithOrigins("https://localhost:5003", "http://localhost:5003").AllowAnyMethod().AllowAnyHeader();
               }));


builder.Services.AddHttpClient();
builder.Services.AddScoped<IBotService, BotService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();
}

app.UseHttpsRedirection();
app.UseCors("AllowLocalhost5003");
app.UseAuthorization();
app.MapControllers();
app.UseStaticFiles();
app.UseRouting();

app.Run();