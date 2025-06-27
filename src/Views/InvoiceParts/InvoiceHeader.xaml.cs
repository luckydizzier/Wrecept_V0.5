using System.Windows.Controls;
using Wrecept.ViewModels;

namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceHeader : UserControl
{
    public InvoiceHeader()
    {
        InitializeComponent();
        Loaded += (_, _) => SupplierNameBox.SearchBox.Focus();
    }
}
