using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Services;

namespace Wrecept.Views.Filters;

public partial class SupplierFilterDialog : UserControl
{
    public SupplierFilterDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => Keyboard.Focus(this);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            App.Services.GetRequiredService<INavigationService>().CloseCurrentView();
            App.Services.GetRequiredService<IStatusService>().SetStatus("Fókusz: főmenü");
        }
        else if (e.Key == Key.Enter)
        {
            App.Services.GetRequiredService<INavigationService>().CloseCurrentView();
        }
    }
}
