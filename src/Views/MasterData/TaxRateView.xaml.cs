using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views.MasterData;

public partial class TaxRateView : UserControl
{
    public TaxRateView()
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
            (DataContext as ViewModels.TaxRateListViewModel)?.AddCommand.Execute(null);
        }
        else if (e.Key == Key.F2)
        {
            (DataContext as ViewModels.TaxRateListViewModel)?.SaveCommand.Execute(null);
        }
        else if (e.Key == Key.Delete)
        {
            (DataContext as ViewModels.TaxRateListViewModel)?.DeleteCommand.Execute(null);
        }
    }
}
