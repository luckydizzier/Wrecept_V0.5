using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.Infrastructure;

namespace Wrecept.Views.Filters;

public partial class DateFilterDialog : UserControl
{
    public DateFilterDialog()
    {
        InitializeComponent();
        Loaded += (_, _) => Keyboard.Focus(this);
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape)
        {
            Infrastructure.AppContext.NavigationService.CloseCurrentView();
            Infrastructure.AppContext.SetStatus("Fókusz: főmenü");
        }
    }
}
