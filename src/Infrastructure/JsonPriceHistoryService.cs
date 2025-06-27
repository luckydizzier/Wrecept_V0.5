using System.Text.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wrecept.Services;

namespace Wrecept.Infrastructure;

public class JsonPriceHistoryService : IPriceHistoryService
{
    private readonly string _path;
    private readonly Dictionary<string, List<PriceEntry>> _data;

    public JsonPriceHistoryService(string? path = null)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            var dir = AppDirectories.GetWritableAppDataDirectory();
            _path = Path.Combine(dir, "prices.json");
        }
        else
        {
            _path = path;
        }
        _data = Load();
    }

    public decimal? GetLatestPrice(string productName)
    {
        if (_data.TryGetValue(productName, out var list) && list.Count > 0)
            return list.OrderByDescending(p => p.Date).First().Price;
        return null;
    }

    public void RecordPrice(string productName, decimal price)
    {
        if (!_data.TryGetValue(productName, out var list))
        {
            list = new List<PriceEntry>();
            _data[productName] = list;
        }
        list.Add(new PriceEntry { Date = DateOnly.FromDateTime(DateTime.Today), Price = price });
        Save();
    }

    private Dictionary<string, List<PriceEntry>> Load()
    {
        if (!File.Exists(_path))
            return new();
        try
        {
            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<Dictionary<string, List<PriceEntry>>>(json) ?? new();
        }
        catch
        {
            return new();
        }
    }

    private void Save()
    {
        var json = JsonSerializer.Serialize(_data);
        File.WriteAllText(_path, json);
    }

    public class PriceEntry
    {
        public DateOnly Date { get; set; }
        public decimal Price { get; set; }
    }
}
