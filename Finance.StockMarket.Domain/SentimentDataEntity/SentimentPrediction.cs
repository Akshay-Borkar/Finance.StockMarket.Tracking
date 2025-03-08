using Microsoft.ML.Data;

namespace Finance.StockMarket.Domain.SentimentDataEntity
{
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public int Prediction { get; set; } // -2 to +2

        [ColumnName("Score")]
        public float Score { get; set; }
    }
}
