using Wrecept.Core.Utilities;
using Xunit;

namespace Wrecept.Tests;

public class HungarianNumberConverterTests
{
    [Theory]
    [InlineData(0, "nulla")]
    [InlineData(1, "egy")]
    [InlineData(123.45, "egyszázhuszonhárom egész negyvenöt század")]
    public void ToText_ShouldReturnHungarianText(decimal value, string expected)
    {
        var text = HungarianNumberConverter.ToText(value, value % 1 == 0 ? 0 : 2);
        Assert.Equal(expected, text);
    }

    [Fact]
    public void GrandTotal_AmountText_ShouldUseConverter()
    {
        var total = new Wrecept.ViewModels.InvoiceEditorViewModel.GrandTotal(100m, 27m);
        Assert.Equal("százhuszonhét", total.AmountText);
    }
}
