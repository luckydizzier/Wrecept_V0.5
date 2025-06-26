using System.Windows;
using System.Windows.Input;

namespace Wrecept.Views.Lookup;

public partial class LookupDialog : Window
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
            DialogResult = false;
            Wrecept.Infrastructure.AppContext.SetStatus("Fókusz: főmenü");
        }
        else if (e.Key == Key.Enter)
        {
            DialogResult = true;
        }
    }
}
