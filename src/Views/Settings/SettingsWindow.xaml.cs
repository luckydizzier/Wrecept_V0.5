using System.Windows;
using System.Windows.Input;
using Wrecept.Infrastructure;

namespace Wrecept.Views.Settings;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        Loaded += (_, _) => Keyboard.Focus(this);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            DialogResult = false;
            Infrastructure.AppContext.SetStatus("Fókusz: főmenü");
        }
    }
}
