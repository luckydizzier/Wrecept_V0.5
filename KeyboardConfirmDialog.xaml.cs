using System.Windows;
using System.Windows.Input;

namespace Wrecept;

public partial class KeyboardConfirmDialog : Window
{
    public KeyboardConfirmDialog()
    {
        InitializeComponent();
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
