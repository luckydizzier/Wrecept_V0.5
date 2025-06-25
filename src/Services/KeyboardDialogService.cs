using System.Windows;

namespace Wrecept.Services;

public class KeyboardDialogService : IKeyboardDialogService
{
    public bool ConfirmNewInvoice()
    {
        var dialog = new KeyboardConfirmDialog();
        return dialog.ShowDialog() == true;
    }

    public bool ConfirmExit()
    {
        var dialog = new KeyboardConfirmDialog("Biztosan kilépsz? (I: Igen, N vagy Esc: Nem)");
        return dialog.ShowDialog() == true;
    }
}
