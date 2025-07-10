using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Threading.Tasks;
using System.Windows.Input;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public abstract partial class InlineCreatorViewModel<T> : ObservableObject
{
    public event Action<T>? Saved;
    public event Action? Canceled;

    public IAsyncRelayCommand SaveCommand { get; }
    public IRelayCommand CancelCommand { get; }
    public IUserCommand SaveUserCommand { get; }
    public IUserCommand CancelUserCommand { get; }

    protected InlineCreatorViewModel()
    {
        SaveCommand = new AsyncRelayCommand(OnSaveAsync);
        CancelCommand = new RelayCommand(OnCancel);
        SaveUserCommand = new UserRelayCommand(OnSaveAsync, new KeyGesture(Key.Enter));
        CancelUserCommand = new UserRelayCommand(() => { OnCancel(); return Task.CompletedTask; }, new KeyGesture(Key.Escape));
    }

    protected abstract Task<T> CreateEntityAsync();

    private async Task OnSaveAsync()
    {
        var entity = await CreateEntityAsync();
        Saved?.Invoke(entity);
    }

    private void OnCancel() => Canceled?.Invoke();
}
