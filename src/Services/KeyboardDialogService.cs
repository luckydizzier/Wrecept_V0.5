using System.Windows;

namespace Wrecept.Services;

public class KeyboardDialogService : IKeyboardDialogService
{
    public bool ConfirmNewInvoice()
    {
        var dialog = new KeyboardConfirmDialog();
        return dialog.ShowDialog() == true;
    }
}
