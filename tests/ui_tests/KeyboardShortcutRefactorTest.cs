using System.IO;
using Xunit;

namespace Wrecept.UiTests;

public class KeyboardShortcutRefactorTest
{
    [Fact]
    public void InvoiceEditorWindow_HasDeprecatedCtrlS()
    {
        var xaml = File.ReadAllText("src/Views/InvoiceEditorWindow.xaml");
        Assert.Contains("DEPRECATED: Ctrl+S", xaml);
    }

    [Fact]
    public void InvoiceItemsGrid_HasDeprecatedShortcuts()
    {
        var xaml = File.ReadAllText("src/Views/InvoiceParts/InvoiceItemsGrid.xaml");
        Assert.Contains("DEPRECATED: F2", xaml);
        Assert.Contains("DEPRECATED: Ctrl+L", xaml);
    }
}
