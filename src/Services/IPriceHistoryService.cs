namespace Wrecept.Services;

public interface IPriceHistoryService
{
    decimal? GetLatestPrice(string productName);
    void RecordPrice(string productName, decimal price);
}
