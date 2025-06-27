namespace Wrecept.Services;

public interface IKeyboardDialogService
{
    bool Confirm(string message);
    bool ConfirmNewInvoice();
    bool ConfirmExit();
}
