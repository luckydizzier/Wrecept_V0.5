using System.Windows.Controls;
using System.Windows.Input;

namespace Wrecept.Views.Dialogs;

public partial class KeyboardConfirmDialog : UserControl
{
    public KeyboardConfirmDialog(string message = "Create new invoice? (I: Yes, N or Esc: No)")
    {
        InitializeComponent();
        PromptText.Text = message;
        Loaded += (_, _) => Keyboard.Focus(this);
    }

    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        base.OnPreviewKeyDown(e);
        if (e.Key == Key.I || e.Key == Key.Enter)
        {
            Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
        else if (e.Key == Key.N || e.Key == Key.Escape)
        {
            Infrastructure.AppContext.NavigationService.CloseCurrentView();
        }
    }
}
