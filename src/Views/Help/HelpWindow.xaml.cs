using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views.Help;

public partial class HelpWindow : UserControl
{
    public HelpWindow()
    {
        InitializeComponent();
    }

    private void Window_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Wrecept.Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
    }
}
