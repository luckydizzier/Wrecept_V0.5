using System.Windows;
using Wrecept.Views.Dialogs;

namespace Wrecept.Services;

public class KeyboardDialogService : IKeyboardDialogService
{
    private static bool Show(string message)
    {
        var control = new KeyboardConfirmDialog(message);
        var window = new Window
        {
            Content = control,
            Owner = Application.Current.MainWindow,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            WindowStyle = WindowStyle.ToolWindow,
            SizeToContent = SizeToContent.WidthAndHeight,
            Title = "Megerősítés"
        };
        return window.ShowDialog() == true;
    }

    public bool Confirm(string message) => Show(message);

    public bool ConfirmNewInvoice() => Confirm("Create new invoice? (I: Yes, N or Esc: No)");

    public bool ConfirmExit() => Confirm("Biztosan kilépsz? (I: Igen, N vagy Esc: Nem)");
}
