using System.Windows;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.ViewModels;
using Wrecept.Services;

namespace Wrecept;

public partial class MainWindow : Window
{
    private readonly IKeyboardDialogService _dialog;
    private readonly INavigationService _navigation;

    public MainWindow(ViewModels.MainWindowViewModel vm)
    {
        InitializeComponent();
        DataContext = vm;
        _dialog = App.Services.GetRequiredService<IKeyboardDialogService>();
        _navigation = App.Services.GetRequiredService<INavigationService>();
        App.Services.GetRequiredService<IStatusService>().StatusMessageSetter = msg => vm.StatusMessage = msg;
        _navigation.SetHost(vm);
    }

    private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Infrastructure.AppContext.InputLocked) return;

        if (MenuBar.IsKeyboardFocusWithin && e.Key == Key.Escape)
        {
            Infrastructure.AppContext.InputLocked = true;
            var confirm = _dialog.ConfirmExit();
            Infrastructure.AppContext.InputLocked = false;
            e.Handled = true;
            if (confirm)
            {
                _navigation.ExitApplication();
            }
        }
    }

}
