using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;

namespace Wrecept.Views.Help;

public partial class HelpWindow : UserControl
{
    public HelpWindow()
    {
        InitializeComponent();
        DataContext = new ViewModels.HelpWindowViewModel(App.Services.GetRequiredService<INavigationService>());
    }

}
