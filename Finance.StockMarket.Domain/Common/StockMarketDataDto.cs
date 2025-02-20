using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.StockMarket.Domain.Common
{
    public class StockMarketDataDto
    {
        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("fullExchangeName")]
        public string FullExchangeName { get; set; }

        [JsonProperty("regularMarketPrice")]
        public decimal RegularMarketPrice { get; set; }

        [JsonProperty("fiftyTwoWeekHigh")]
        public decimal FiftyTwoWeekHigh { get; set; }

        [JsonProperty("fiftyTwoWeekLow")]
        public decimal FiftyTwoWeekLow { get; set; }

        [JsonProperty("regularMarketDayHigh")]
        public decimal RegularMarketDayHigh { get; set; }

        [JsonProperty("regularMarketDayLow")]
        public decimal RegularMarketDayLow { get; set; }

        [JsonProperty("regularMarketVolume")]
        public long RegularMarketVolume { get; set; }

        [JsonProperty("longName")]
        public string LongName { get; set; }

        [JsonProperty("shortName")]
        public string ShortName { get; set; }

        [JsonProperty("previousClose")]
        public decimal PreviousClose { get; set; }
    }

    public class ChartResult
    {
        [JsonProperty("meta")]
        public StockMarketDataDto Meta { get; set; }
    }

    public class ChartData
    {
        [JsonProperty("result")]
        public List<ChartResult> Result { get; set; }
    }

    public class StockApiResponse
    {
        [JsonProperty("chart")]
        public ChartData Chart { get; set; }
    }
}
