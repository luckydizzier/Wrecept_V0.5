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
}
