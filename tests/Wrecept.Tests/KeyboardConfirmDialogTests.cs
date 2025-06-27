using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Wrecept.Views.Dialogs;
using Xunit;

namespace Wrecept.Tests;

public class KeyboardConfirmDialogTests
{
    [StaFact]
    public void Dialog_ShouldBeFocusable()
    {
        var dialog = new KeyboardConfirmDialog();
        Assert.True(dialog.Focusable);
    }

    [StaFact]
    public void OnPreviewKeyDown_ShouldSetDialogResult()
    {
        var dialog = new KeyboardConfirmDialog();
        var window = new Window { Content = dialog };
        window.Show();
        var method = typeof(KeyboardConfirmDialog).GetMethod("OnPreviewKeyDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var source = new HwndSource(new HwndSourceParameters());
        var args = new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, Key.I)
        {
            RoutedEvent = Keyboard.PreviewKeyDownEvent
        };
        method.Invoke(dialog, new object[] { args });
        Assert.True(window.DialogResult);
        window.Close();
    }
}
