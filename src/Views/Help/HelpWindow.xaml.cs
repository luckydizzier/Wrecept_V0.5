using System.Windows;
using System.Windows.Input;

namespace Wrecept.Views.Help;

public partial class HelpWindow : Window
{
    public HelpWindow()
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
}
