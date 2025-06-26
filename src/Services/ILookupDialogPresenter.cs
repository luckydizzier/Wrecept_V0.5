namespace Wrecept.Services;

public interface ILookupDialogPresenter
{
    bool? ShowDialog<T>(Wrecept.ViewModels.LookupDialogViewModel<T> vm);
}
