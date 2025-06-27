using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;

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
            App.Services.GetRequiredService<INavigationService>().CloseCurrentView();
            App.Services.GetRequiredService<IStatusService>().SetStatus("Fókusz: főmenü");
        }
    }
}
