using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;

namespace Wrecept.Views;

public partial class OnboardingOverlay : UserControl
{
    public OnboardingOverlay()
    {
        InitializeComponent();
        var vm = new ViewModels.OnboardingOverlayViewModel(App.Services.GetRequiredService<INavigationService>());
        vm.CloseWindow = () => Window.GetWindow(this)?.Close();
        DataContext = vm;
    }

}
