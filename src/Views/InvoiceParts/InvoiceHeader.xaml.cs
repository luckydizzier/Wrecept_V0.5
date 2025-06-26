using System.Windows.Controls;

namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceHeader : UserControl
{
    public InvoiceHeader()
    {
        InitializeComponent();
        Loaded += (_, _) => SupplierNameBox.Focus();
    }
}
