using Finance.StockMarket.Application.Contracts.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Infrastructure.Logging
{
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _loggerFactory;

        public LoggerAdapter(ILoggerFactory loggerFactory)
        {
            this._loggerFactory = loggerFactory.CreateLogger<T>();
        }
        public void LogError(string message, params object[] args)
        {
            _loggerFactory.LogError(message, args);
        }

        public void LogInformation(string message, params object[] args)
        {
            _loggerFactory.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _loggerFactory.LogWarning(message, args);
        }
    }
}
