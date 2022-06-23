using Microsoft.EntityFrameworkCore;
using Packt.Shared;
using ParkingLotService.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
string databasePath = Path.Combine("..", "ParkingSpot.db");
builder.Services.AddDbContext<ParkingLot>(options => 
  options.UseSqlite($"Data Source={databasePath}"));
builder.Services.AddCors();
builder.Services.AddControllers().AddXmlDataContractSerializerFormatters().AddXmlSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IParkingSpotRepository, ParkingSpotRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(configurePolicy: options => 
{
  options.WithMethods("GET", "POST", "PUT", "DELETE");
  options.WithOrigins(
    "https://localhost:7110" // for MVC client
  );
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
