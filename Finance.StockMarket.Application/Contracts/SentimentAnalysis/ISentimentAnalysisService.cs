﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Application.Contracts.SentimentAnalysis
{
    public interface ISentimentAnalysisService
    {
        string PredictSentiment(string text);
    }
}
