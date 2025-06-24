using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Wrecept.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    [ObservableProperty]
    private string _greeting = "Üdvözlet";

    [RelayCommand]
    private void ShowGreeting()
    {
        // TODO: implement action
    }
}
