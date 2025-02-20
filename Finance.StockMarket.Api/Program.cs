using Finance.StockMarket.Api.Middleware;
using Finance.StockMarket.Application;
using Finance.StockMarket.Application.SignalRHub;
using Finance.StockMarket.Identity;
using Finance.StockMarket.Infrastructure;
using Finance.StockMarket.Infrastructure.HangfireJob;
using Finance.StockMarket.Persistence;
using Hangfire;

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
                builder.WithOrigins("http://localhost:3000/")
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

#region Enable Hangfire Dashboard
    app.UseHangfireDashboard();
    app.MapHangfireDashboard();
#endregion

#region JobSchedulerService and call ScheduleJobs()
    using (var scope = app.Services.CreateScope())
    {
        var jobScheduler = scope.ServiceProvider.GetRequiredService<JobSchedulerService>();
        jobScheduler.ScheduleJobs();  // This schedules all recurring jobs
    }
#endregion
app.UseRouting();
app.UseCors("all");
#region SignalR Hub
app.UseEndpoints(endpoints => endpoints.MapHub<StockPriceHub>("/stockMarketHub"));
#endregion

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
