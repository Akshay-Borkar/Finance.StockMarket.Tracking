using Finance.StockMarket.Application.Contracts.Hangfire.BackgroundJobService;
using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Infrastructure.HangfireJob
{
    public class BackgroundJobService : IBackgroundJobService
    {
        public string Enqueue(Expression<Action> methodCall)
        {
            return Hangfire.BackgroundJob.Enqueue(methodCall);
        }

        public void Schedule(Expression<Action> methodCall, TimeSpan delay)
        {
            Hangfire.BackgroundJob.Schedule(methodCall, delay);
        }

        public void AddOrUpdateRecurringJob(string recurringJobId, Expression<Action> methodCall, string cronExpression)
        {
            RecurringJob.AddOrUpdate(recurringJobId, methodCall, cronExpression);
        }

        public void RemoveIfExists(string recurringJobId)
        {
            RecurringJob.RemoveIfExists(recurringJobId);
        }
    }
}
