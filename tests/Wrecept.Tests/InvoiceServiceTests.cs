using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceServiceTests
{
    [Fact]
    public void Constructor_ShouldCreateInstance()
    {
        var service = new InvoiceService();
        Assert.NotNull(service);
    }
}
