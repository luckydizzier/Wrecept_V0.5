using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace Wrecept.Services;

public static class VisualFeedback
{
    public static void FlashSuccess(Control control) => _ = Flash(control, Colors.LightGreen);
    public static void FlashWarning(Control control) => _ = Flash(control, Colors.Gold);
    public static void FlashError(Control control) => _ = Flash(control, Colors.Red);

    private static async Task Flash(Control control, Color color)
    {
        var dispatcher = control.Dispatcher;
        var original = (control.Background as SolidColorBrush)?.Color ?? Colors.White;
        var brush = new SolidColorBrush(color);
        for (int i = 0; i < 2; i++)
        {
            dispatcher.Invoke(() => control.Background = brush);
            await Task.Delay(100);
            dispatcher.Invoke(() => control.Background = new SolidColorBrush(original));
            await Task.Delay(100);
        }
    }
}
