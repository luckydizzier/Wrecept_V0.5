using System.Windows;
using System.Windows.Input;

namespace Wrecept.Views.Dialogs;

public partial class KeyboardConfirmDialog : Window
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
            DialogResult = true;
        }
        else if (e.Key == Key.N || e.Key == Key.Escape)
        {
            DialogResult = false;
        }
    }
}
