using System;
using System.Windows;
using Xunit;

namespace Wrecept.Tests;

public class ThemeLoadTests
{
    [StaFact]
    public void ApplyTheme_ShouldLoadResourceDictionary()
    {
        var ex = Record.Exception(() => Wrecept.App.ApplyTheme("Light"));
        Assert.Null(ex);
    }

    [StaFact]
    public void TextBoxCreation_ShouldNotThrowAfterThemeApplied()
    {
        Wrecept.App.ApplyTheme("Light");
        var ex = Record.Exception(() => new System.Windows.Controls.TextBox());
        Assert.Null(ex);
    }
}
