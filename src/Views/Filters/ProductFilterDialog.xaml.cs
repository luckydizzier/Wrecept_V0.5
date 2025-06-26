using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Infrastructure;

namespace Wrecept.Views.Filters;

public partial class ProductFilterDialog : UserControl
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
            Infrastructure.AppContext.NavigationService.CloseCurrentView();
            Infrastructure.AppContext.SetStatus("Fókusz: főmenü");
        }
        else if (e.Key == Key.Enter)
        {
            // Enter handled by default button
        }
    }
}
