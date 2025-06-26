using System.Windows;
using Wrecept.Views.Lookup;

namespace Wrecept.Services;

public class LookupDialogPresenter : ILookupDialogPresenter
{
    public bool? ShowDialog<T>(ViewModels.LookupDialogViewModel<T> vm)
    {
        var dlg = new LookupDialog
        {
            DataContext = vm,
            Owner = Application.Current.MainWindow
        };
        return dlg.ShowDialog();
    }
}
