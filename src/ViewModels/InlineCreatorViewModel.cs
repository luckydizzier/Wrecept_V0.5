using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.ViewModels;

public abstract partial class InlineCreatorViewModel<T> : ObservableObject
{
    public event Action<T>? Saved;
    public event Action? Canceled;

    public IRelayCommand SaveCommand { get; }
    public IRelayCommand CancelCommand { get; }

    protected InlineCreatorViewModel()
    {
        SaveCommand = new RelayCommand(OnSave);
        CancelCommand = new RelayCommand(OnCancel);
    }

    protected abstract Task<T> CreateEntityAsync();

    private async void OnSave()
    {
        var entity = await CreateEntityAsync();
        Saved?.Invoke(entity);
    }

    private void OnCancel() => Canceled?.Invoke();
}
