using System.Windows;
using Wrecept.Core.Plugins;

namespace Wrecept.Plugin.Greeting;

public class GreetingPlugin : IMenuPlugin
{
    public string MenuHeader => "Köszöntés";

    public void Execute()
    {
        var win = new GreetingWindow { Owner = Application.Current.MainWindow };
        win.ShowDialog();
    }
}
