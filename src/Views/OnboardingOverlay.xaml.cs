using System.Windows;

namespace Wrecept.Views;

public partial class OnboardingOverlay : Window
{
    public OnboardingOverlay()
    {
        InitializeComponent();
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
