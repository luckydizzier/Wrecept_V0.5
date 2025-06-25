using System.Windows;
using System.Windows.Input;
using Wrecept.Infrastructure;

namespace Wrecept.Views.MasterData;

public partial class SupplierView : Window
{
    public SupplierView()
    {
        InitializeComponent();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            DialogResult = false;
            Infrastructure.AppContext.SetStatus("Fókusz: főmenü");
        }
        else if (e.Key == Key.Insert)
        {
            (DataContext as ViewModels.SupplierListViewModel)?.AddCommand.Execute(null);
        }
        else if (e.Key == Key.F2)
        {
            (DataContext as ViewModels.SupplierListViewModel)?.SaveCommand.Execute(null);
        }
        else if (e.Key == Key.Delete)
        {
            (DataContext as ViewModels.SupplierListViewModel)?.DeleteCommand.Execute(null);
        }
    }
}
