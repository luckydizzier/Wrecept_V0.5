using System.Windows;
using System.Windows.Input;

namespace Wrecept.Views.Filters;

public partial class DateFilterDialog : Window
{
    public DateFilterDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => Keyboard.Focus(this);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            DialogResult = false;
        }
    }
}
