using System.Windows;
using System.Windows.Input;

namespace Wrecept.Views.MasterData;

public partial class ProductView : Window
{
    public ProductView()
    {
        InitializeComponent();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            DialogResult = false;
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
