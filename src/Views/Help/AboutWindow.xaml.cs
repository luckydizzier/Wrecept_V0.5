using System.Windows.Controls;
using System.Windows.Input;

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
            Wrecept.Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        Wrecept.Infrastructure.AppContext.NavigationService.CloseCurrentView();
    }
}
