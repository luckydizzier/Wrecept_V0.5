using System.Windows;
using Wrecept.Views.Lookup;

namespace Wrecept.Services;

public class LookupDialogPresenter : ILookupDialogPresenter
{
    public bool? ShowDialog<T>(ViewModels.LookupDialogViewModel<T> vm)
    {
        var control = new LookupDialog { DataContext = vm };
        var window = new Window
        {
            Content = control,
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            WindowStyle = WindowStyle.ToolWindow,
            SizeToContent = SizeToContent.WidthAndHeight,
            Title = "Keresés"
        };
        return window.ShowDialog();
    }
}
