using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public class OnboardingOverlayViewModel
{
    public IUserCommand CloseCommand { get; }
    public Action? CloseWindow { get; set; }

    public OnboardingOverlayViewModel(INavigationService navigation)
    {
        CloseCommand = new UserRelayCommand(() =>
        {
            navigation.CloseCurrentView();
            CloseWindow?.Invoke();
            return Task.CompletedTask;
        }, new KeyGesture(Key.Escape));
    }
}
