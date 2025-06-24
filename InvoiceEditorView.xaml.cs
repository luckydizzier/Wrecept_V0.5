using System.Windows;

namespace Wrecept;

public partial class InvoiceEditorView : Window
{
    public InvoiceEditorView()
    {
        InitializeComponent();
    }

    private void OnClose(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
