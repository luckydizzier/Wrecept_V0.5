using System.IO;
using Xunit;

namespace Wrecept.UiTests;

public class SidebarPanelRefactorTest
{
    [Fact]
    public void InvoiceSidebar_ShouldContainOnlyList()
    {
        var xaml = File.ReadAllText("src/Views/InvoiceParts/InvoiceSidebar.xaml");
        Assert.DoesNotContain("<TextBox", xaml);
        Assert.DoesNotContain("<DatePicker", xaml);
        Assert.DoesNotContain("<ComboBox", xaml);
        Assert.Contains("📄 Számlák", xaml);
        Assert.Contains("InvoiceList", xaml);
    }
}
