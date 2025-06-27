using System.Windows;
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
            Window.GetWindow(this)!.DialogResult = true;
            e.Handled = true;
        }
        else if (e.Key == Key.N || e.Key == Key.Escape)
        {
            Window.GetWindow(this)!.DialogResult = false;
            e.Handled = true;
        }
    }
}
