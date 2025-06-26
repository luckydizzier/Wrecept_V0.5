using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;

namespace Wrecept.ViewModels;

public abstract partial class InlineCreatorViewModel<T> : ObservableObject
{
    public event Action<T>? Saved;
    public event Action? Canceled;

    public IAsyncRelayCommand SaveCommand { get; }
    public IRelayCommand CancelCommand { get; }

    protected InlineCreatorViewModel()
    {
        SaveCommand = new AsyncRelayCommand(OnSaveAsync);
        CancelCommand = new RelayCommand(OnCancel);
    }

    protected abstract Task<T> CreateEntityAsync();

    private async Task OnSaveAsync()
    {
        var entity = await CreateEntityAsync();
        Saved?.Invoke(entity);
    }

    private void OnCancel() => Canceled?.Invoke();
}
