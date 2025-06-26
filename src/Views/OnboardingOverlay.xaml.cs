using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views;

public partial class OnboardingOverlay : UserControl
{
    public OnboardingOverlay()
    {
        InitializeComponent();
    }

    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        Infrastructure.AppContext.NavigationService.CloseCurrentView();
    }
}
