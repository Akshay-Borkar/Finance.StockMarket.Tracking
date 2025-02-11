using Finance.StockMarket.Application;
using Finance.StockMarket.Infrastructure;
using Finance.StockMarket.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddCors(options => { 
    options.AddPolicy("all", builder => 
            builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
});
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
