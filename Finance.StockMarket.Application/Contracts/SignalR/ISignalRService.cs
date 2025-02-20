namespace Finance.StockMarket.Application.Contracts.SignalRHub
{
    public interface ISignalRService
    {
        Task SendStockPriceUpdate(string ticker, string price);
    }
}
