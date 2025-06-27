using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;

namespace Wrecept.Views.MasterData;

public partial class SupplierView : UserControl
{
    public SupplierView()
    {
        InitializeComponent();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (Infrastructure.AppContext.InputLocked) return;
        if (e.Key == Key.Escape)
        {
            App.Services.GetRequiredService<INavigationService>().CloseCurrentView();
        }
    }
}
