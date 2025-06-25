using System.Windows;
using System.Windows.Input;

namespace Wrecept.Views.Filters;

public partial class ProductFilterDialog : Window
{
    public ProductFilterDialog()
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
        else if (e.Key == Key.Enter)
        {
            DialogResult = true;
        }
    }
}
