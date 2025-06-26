using System.Windows;
using System.Windows.Input;
using WreceptAppContext = Wrecept.Infrastructure.AppContext;
using Wrecept.ViewModels;

namespace Wrecept;

public partial class MainWindow : Window
{

    public MainWindow()
    {
        InitializeComponent();

        if (DataContext is ViewModels.MainWindowViewModel vm)
        {
            WreceptAppContext.StatusMessageSetter = msg => vm.StatusMessage = msg;
            WreceptAppContext.NavigationService.SetHost(vm);
        }
    }

    private void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (Infrastructure.AppContext.InputLocked) return;

        if (MenuBar.IsKeyboardFocusWithin && e.Key == Key.Escape)
        {
            Infrastructure.AppContext.InputLocked = true;
            var confirm = WreceptAppContext.DialogService.ConfirmExit();
            Infrastructure.AppContext.InputLocked = false;
            e.Handled = true;
            if (confirm)
            {
                WreceptAppContext.NavigationService.ExitApplication();
            }
        }
    }

}
