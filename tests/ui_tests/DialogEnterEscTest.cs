using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Wrecept.Views.Dialogs;
using Xunit;

namespace Wrecept.UiTests;

public class DialogEnterEscTest
{
    [StaFact]
    public void EnterKey_ShouldConfirm()
    {
        var dialog = new KeyboardConfirmDialog();
        var window = new Window { Content = dialog };
        window.Show();
        var method = typeof(KeyboardConfirmDialog).GetMethod("OnPreviewKeyDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var source = new HwndSource(new HwndSourceParameters());
        var args = new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, Key.Enter) { RoutedEvent = Keyboard.PreviewKeyDownEvent };
        method.Invoke(dialog, new object[] { args });
        Assert.True(window.DialogResult);
        window.Close();
    }

    [StaFact]
    public void EscapeKey_ShouldCancel()
    {
        var dialog = new KeyboardConfirmDialog();
        var window = new Window { Content = dialog };
        window.Show();
        var method = typeof(KeyboardConfirmDialog).GetMethod("OnPreviewKeyDown", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var source = new HwndSource(new HwndSourceParameters());
        var args = new KeyEventArgs(Keyboard.PrimaryDevice, source, 0, Key.Escape) { RoutedEvent = Keyboard.PreviewKeyDownEvent };
        method.Invoke(dialog, new object[] { args });
        Assert.False(window.DialogResult);
        window.Close();
    }
}
