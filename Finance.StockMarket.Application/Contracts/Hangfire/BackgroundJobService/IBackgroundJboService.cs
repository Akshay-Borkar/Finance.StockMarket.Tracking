using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Contracts.Hangfire.BackgroundJobService
{
    public interface IBackgroundJobService
    {
        string Enqueue(Expression<Action> methodCall);
        void Schedule(Expression<Action> methodCall, TimeSpan delay);
        void AddOrUpdateRecurringJob(string recurringJobId, Expression<Action> methodCall, string cronExpression);
        void RemoveIfExists(string recurringJobId);
    }
}
