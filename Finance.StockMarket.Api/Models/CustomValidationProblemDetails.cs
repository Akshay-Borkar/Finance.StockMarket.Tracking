using Microsoft.AspNetCore.Mvc;

namespace Finance.StockMarket.Api.Models
{
    public class CustomValidationProblemDetails: ProblemDetails
    {
        public IDictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
    }
}
