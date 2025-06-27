using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views.MasterData;

public partial class UnitView : UserControl
{
    public UnitView()
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
            (DataContext as ViewModels.UnitListViewModel)?.AddCommand.Execute(null);
        }
        else if (e.Key == Key.F2)
        {
            (DataContext as ViewModels.UnitListViewModel)?.SaveCommand.Execute(null);
        }
        else if (e.Key == Key.Delete)
        {
            (DataContext as ViewModels.UnitListViewModel)?.DeleteCommand.Execute(null);
        }
    }
}
