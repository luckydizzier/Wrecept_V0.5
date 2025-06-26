using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Infrastructure;

namespace Wrecept.Views.Filters;

public partial class ProductGroupFilterDialog : UserControl
{
    public ProductGroupFilterDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => Keyboard.Focus(this);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            DialogResult = false;
            Infrastructure.AppContext.SetStatus("Fókusz: főmenü");
        }
        else if (e.Key == Key.Enter)
        {
            DialogResult = true;
        }
    }
}
