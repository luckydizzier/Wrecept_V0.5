using System.Windows;
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
            CloseOverlay();
        }
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        CloseOverlay();
    }

    private void CloseOverlay()
    {
        Infrastructure.AppContext.NavigationService.CloseCurrentView();
        Window.GetWindow(this)?.Close();
    }
}
