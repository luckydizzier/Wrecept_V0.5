using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.Services;

public class UserRelayCommand : AsyncRelayCommand, IUserCommand
{
    public KeyGesture Gesture { get; }

    public UserRelayCommand(Func<Task> execute, KeyGesture gesture)
        : base(execute)
    {
        Gesture = gesture;
    }
}
