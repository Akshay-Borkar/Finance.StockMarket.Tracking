using Finance.StockMarket.Application.Contracts.SentimentAnalysis;
using Finance.StockMarket.Domain.SentimentDataEntity;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Infrastructure.SentimentAnalysis
{
    public class SentimentAnalysisService : ISentimentAnalysisService
    {
        //private readonly MLContext _mlContext;
        //private readonly ITransformer _model;
        //private readonly PredictionEngine<SentimentData, SentimentPrediction> _predictionEngine;
        //private readonly Dictionary<int, string> _sentimentLabels = new()
        //{
        //    { -2, "Very Negative" },
        //    { -1, "Negative" },
        //    {  0, "Neutral" },
        //    {  1, "Positive" },
        //    {  2, "Very Positive" }
        //};
        private readonly HttpClient _httpClient;
        private readonly Dictionary<string, string> _trainingData;
        private const string YAHOO_NEWS_URL = "https://feeds.finance.yahoo.com/rss/2.0/headline?s={0}&region=US&lang=en-US";

        public SentimentAnalysisService(HttpClient httpClient)
        {
            //_mlContext = new MLContext();

            //var data = _mlContext.Data.LoadFromTextFile<SentimentData>("Data/sentimentdata.csv", separatorChar: ',', hasHeader: true);
            //var pipeline = _mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.NewsText))
            //    .Append(_mlContext.Transforms.Conversion.MapValueToKey("Label"))
            //    .Append(_mlContext.MulticlassClassification.Trainers.SdcaMaximumEntropy())
            //    .Append(_mlContext.Transforms.Conversion.MapKeyToValue("PredictedLabel"));

            //_model = pipeline.Fit(data);
            //_predictionEngine = _mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(_model);
            _httpClient = httpClient;
            _trainingData = new Dictionary<string, string>();
        }

        public string PredictSentiment(string text)
        {
            //var input = new SentimentData { NewsText = text };
            //var prediction = _predictionEngine.Predict(input);
            //return _sentimentLabels.ContainsKey(prediction.Prediction) ? _sentimentLabels[prediction.Prediction] : "Unknown";

            return "Unknown";
        }
    }
}
