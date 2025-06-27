using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views.MasterData;

public partial class ProductGroupView : UserControl
{
    public ProductGroupView()
    {
        InitializeComponent();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (Infrastructure.AppContext.InputLocked) return;
        if (e.Key == Key.Escape)
        {
            Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
    }
}
