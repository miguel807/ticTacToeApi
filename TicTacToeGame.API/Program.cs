using TicTacToeGame.API.Application;
using TicTacToeGame.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSignalR();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<Dictionary<string, char[,]>>();
builder.Services.AddSingleton<Dictionary<string, int>>();
builder.Services.AddSingleton<Dictionary<string, string>>();
builder.Services.AddSingleton<Dictionary<string, int>>();
builder.Services.AddSingleton<Dictionary<string, string>>();
builder.Services.AddSingleton<LimitPlayerByRoomUseCase>();
builder.Services.AddSingleton<TableInitializerUseCase>();
builder.Services.AddSingleton<GameDataInitializer>();
builder.Services.AddSingleton<DetermineWinner>();
builder.Services.AddSingleton<CleanGameState>();
builder.Services.AddSingleton<PlayerTurnControlUseCase>();
builder.Services.AddSingleton<ValidateMoveUseCase>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("MyAllowSpecificOrigins");

app.UseAuthorization();

app.MapControllers();

app.MapHub<TicTacToeGameHub>("/game");

app.Run();
