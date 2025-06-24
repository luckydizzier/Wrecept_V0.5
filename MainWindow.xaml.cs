using System.Windows;

namespace Wrecept;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        if (DataContext is ViewModels.MainWindowViewModel vm)
        {
            _ = vm.LoadInvoicesAsync();
        }
    }
}
