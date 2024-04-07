using Microsoft.EntityFrameworkCore;
using Users.API.Domain;
using Users.API.Infrastucture;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));
builder.Services.AddScoped<IUserRepository, PostgresUserRepository>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                 options.UseNpgsql("Host = localhost; Port = 5444; Database = FirstNetApp; Username = enterprisedb; Password = admin"));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
