using System.IO;
using Wrecept.Infrastructure;
using Xunit;

namespace Wrecept.Tests;

public class JsonPriceHistoryServiceTests
{
    [Fact]
    public void RecordPrice_ShouldPersistAndReturnLatest()
    {
        var dir = Path.GetTempPath();
        var path = Path.Combine(dir, "prices_test.json");
        if (File.Exists(path)) File.Delete(path);
        var service = new JsonPriceHistoryServiceMock(path);

        service.RecordPrice("P", 10m);
        service.RecordPrice("P", 12m);

        var latest = service.GetLatestPrice("P");
        Assert.Equal(12m, latest);
    }

    private class JsonPriceHistoryServiceMock : JsonPriceHistoryService
    {
        public JsonPriceHistoryServiceMock(string file) : base(file) {}
    }
}
