using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Infrastructure;

namespace Wrecept.Views.MasterData;

public partial class ProductView : UserControl
{
    public ProductView()
    {
        InitializeComponent();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (Infrastructure.AppContext.InputLocked) return;
        if (e.Key == Key.Escape)
        {
            Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
        else if (e.Key == Key.Insert)
        {
            (DataContext as ViewModels.ProductListViewModel)?.AddCommand.Execute(null);
        }
        else if (e.Key == Key.F2)
        {
            (DataContext as ViewModels.ProductListViewModel)?.SaveCommand.Execute(null);
        }
        else if (e.Key == Key.Delete)
        {
            (DataContext as ViewModels.ProductListViewModel)?.DeleteCommand.Execute(null);
        }
    }
}
