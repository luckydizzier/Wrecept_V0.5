using FluentAssertions;
using Wrecept.Core.Services;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceServiceTests
{
    [Fact]
    public void Dummy_ShouldPass()
    {
        var service = new InvoiceService();
        service.Should().NotBeNull();
    }

    [Fact]
    public void Dummy_ShouldFail()
    {
        var service = new InvoiceService();
        service.Should().BeNull();
    }
}
