using System.Windows;
using System.Windows.Input;
using Wrecept.Infrastructure;

namespace Wrecept.Views.MasterData;

public partial class ProductView : Window
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
            DialogResult = false;
            Infrastructure.AppContext.SetStatus("Fókusz: főmenü");
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
