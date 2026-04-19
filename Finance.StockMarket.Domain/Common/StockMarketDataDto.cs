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

    public class OhlcvQuote
    {
        [JsonProperty("open")]
        public List<decimal?> Open { get; set; } = [];

        [JsonProperty("high")]
        public List<decimal?> High { get; set; } = [];

        [JsonProperty("low")]
        public List<decimal?> Low { get; set; } = [];

        [JsonProperty("close")]
        public List<decimal?> Close { get; set; } = [];

        [JsonProperty("volume")]
        public List<long?> Volume { get; set; } = [];
    }

    public class ChartIndicators
    {
        [JsonProperty("quote")]
        public List<OhlcvQuote> Quote { get; set; } = [];
    }

    public class ChartResult
    {
        [JsonProperty("meta")]
        public StockMarketDataDto Meta { get; set; }

        [JsonProperty("timestamp")]
        public List<long> Timestamp { get; set; } = [];

        [JsonProperty("indicators")]
        public ChartIndicators Indicators { get; set; } = new();
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

    public class OhlcvBar
    {
        public long Time { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }
        public long Volume { get; set; }
    }
}
