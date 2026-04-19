using Finance.StockMarket.Api.Middleware;
using Finance.StockMarket.Application;
using Finance.StockMarket.Application.SignalRHub;
using Finance.StockMarket.Identity;
using Finance.StockMarket.Identity.DbContext;
using Finance.StockMarket.Infrastructure;
using Finance.StockMarket.Infrastructure.HangfireJob;
using Finance.StockMarket.Persistence;
using Finance.StockMarket.Domain.DatabaseContext;
using Hangfire;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Add services to the container.
    builder.Services.AddApplicationServices();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddPersistenceServices(builder.Configuration);
    builder.Services.AddIdentityServices(builder.Configuration);

    builder.Services.AddControllers();

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("all", builder =>
                builder.WithOrigins("http://localhost:4200", "http://localhost:3000")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
    });
    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddHttpClient();
#endregion

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<FinanceStockMarketDatabaseContext>().Database.Migrate();
    scope.ServiceProvider.GetRequiredService<FinanceStockMarketIdentityDBContext>().Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("all");
if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// All endpoint mappings must come after UseAuthorization
app.UseHangfireDashboard();
app.MapHangfireDashboard();
app.MapHub<StockPriceHub>("/stockMarketHub");
app.MapControllers();

#region JobSchedulerService and call ScheduleJobs()
    // Must run after UseHangfireDashboard() so JobStorage is initialized
    using (var scope = app.Services.CreateScope())
    {
        var jobScheduler = scope.ServiceProvider.GetRequiredService<JobSchedulerService>();
        jobScheduler.ScheduleJobs();
    }
#endregion

app.Run();
