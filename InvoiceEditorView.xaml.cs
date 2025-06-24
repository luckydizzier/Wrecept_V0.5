using System.Windows;

namespace Wrecept;

public partial class InvoiceEditorView : Window
{
    public InvoiceEditorView()
    {
        InitializeComponent();
    }

    protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
    {
        base.OnPreviewKeyDown(e);
        if (e.Key == System.Windows.Input.Key.Escape)
        {
            if (DataContext is ViewModels.InvoiceEditorViewModel vm)
            {
                vm.CancelEdit();
            }
            Close();
            e.Handled = true;
        }
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        if (DataContext is ViewModels.InvoiceEditorViewModel vm)
        {
            vm.CancelEdit();
        }
        Close();
    }
}
