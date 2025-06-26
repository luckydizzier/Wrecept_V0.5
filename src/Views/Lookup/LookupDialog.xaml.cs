using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views.Lookup;

public partial class LookupDialog : UserControl
{
    public LookupDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => SearchBox.Focus();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Wrecept.Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
        else if (e.Key == Key.Enter)
        {
            Wrecept.Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
    }
}
