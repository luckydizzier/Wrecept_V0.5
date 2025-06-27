using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Services;

namespace Wrecept.Views.Help;

public partial class AboutWindow : UserControl
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            App.Services.GetRequiredService<INavigationService>().CloseCurrentView();
        }
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        App.Services.GetRequiredService<INavigationService>().CloseCurrentView();
    }
}
