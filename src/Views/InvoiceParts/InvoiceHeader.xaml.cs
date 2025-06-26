using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.ViewModels;

namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceHeader : UserControl
{
    public InvoiceHeader()
    {
        InitializeComponent();
        Loaded += (_, _) => SupplierNameBox.Focus();
    }

    private void SupplierNameBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter && DataContext is InvoiceHeaderViewModel vm)
        {
            if (vm.TryOpenSupplierCreator())
                e.Handled = true;
        }
    }
}
