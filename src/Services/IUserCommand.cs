using System.Threading.Tasks;
using System.Windows.Input;

namespace Wrecept.Services;

public interface IUserCommand
{
    KeyGesture Gesture { get; }
    Task ExecuteAsync();
}
