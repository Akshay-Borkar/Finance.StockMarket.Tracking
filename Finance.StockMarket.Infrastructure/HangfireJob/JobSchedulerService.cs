using Finance.StockMarket.Application.Contracts.Hangfire.BackgroundJobService;
using Finance.StockMarket.Application.Contracts.Hangfire.StockPriceUpdationJob;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Infrastructure.HangfireJob
{
    public class JobSchedulerService
    {
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IStockPriceUpdateJob _stockPriceUpdateJob;

        public JobSchedulerService(IBackgroundJobService backgroundJobService, IStockPriceUpdateJob stockPriceUpdateJob)
        {
            _backgroundJobService = backgroundJobService;
            _stockPriceUpdateJob = stockPriceUpdateJob;
        }

        public void ScheduleJobs()
        {
            // Schedule the job to run every hour
            _backgroundJobService.AddOrUpdateRecurringJob(
                "UpdateStockPricesMinutely",
                () => _stockPriceUpdateJob.UpdateStockPriceAsync(),
                Cron.Minutely());

            // Schedule the job to run every hour
            _backgroundJobService.AddOrUpdateRecurringJob(
                "UpdateStockPricesHourly",
                () => _stockPriceUpdateJob.UpdateStockPriceAsync(),
                Cron.Hourly());

            // Schedule another job to run daily at midnight
            _backgroundJobService.AddOrUpdateRecurringJob(
                "UpdateStockPricesDaily",
                () => _stockPriceUpdateJob.UpdateStockPriceAsync(),
                Cron.Daily());

            // Schedule a job to run every Monday at 9 AM
            _backgroundJobService.AddOrUpdateRecurringJob(
                "UpdateStockPricesWeekly",
                () => _stockPriceUpdateJob.UpdateStockPriceAsync(),
                Cron.Weekly(DayOfWeek.Monday, 9, 0));
        }
    }
}
