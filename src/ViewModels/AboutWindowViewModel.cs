using System.Threading.Tasks;
using System.Windows.Input;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public class AboutWindowViewModel
{
    public IUserCommand CloseCommand { get; }

    public AboutWindowViewModel(INavigationService navigation)
    {
        CloseCommand = new UserRelayCommand(() => { navigation.CloseCurrentView(); return Task.CompletedTask; }, new KeyGesture(Key.Escape));
    }
}
