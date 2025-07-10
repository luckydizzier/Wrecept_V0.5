using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;

namespace Wrecept.Views.Help;

public partial class AboutWindow : UserControl
{
    public AboutWindow()
    {
        InitializeComponent();
        DataContext = new ViewModels.AboutWindowViewModel(App.Services.GetRequiredService<INavigationService>());
    }
}
