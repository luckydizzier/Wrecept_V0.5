using System.Windows;
using System.Windows.Input;

namespace Wrecept.Views;

public partial class OnboardingOverlay : Window
{
    public OnboardingOverlay()
    {
        InitializeComponent();
    }

    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            DialogResult = true;
        }
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
