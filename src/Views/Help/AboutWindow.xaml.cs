using System.Windows;

namespace Wrecept.Views.Help;

public partial class AboutWindow : Window
{
    public AboutWindow()
    {
        InitializeComponent();
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}
