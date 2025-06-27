using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.ViewModels;

namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceHeader : UserControl
{
    public InvoiceHeader()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            SupplierNameBox.SearchBox.Focus();
            AccessKeyManager.Register("N", SupplierNameBox);
            AccessKeyManager.Register("C", AddressBox);
            AccessKeyManager.Register("A", TaxNumberBox);
            AccessKeyManager.Register("P", SerialBox);
            AccessKeyManager.Register("D", IssueDateBox);
            AccessKeyManager.Register("M", PaymentMethodBox);
            AccessKeyManager.Register("T", TransactionBox);
        };
    }
}
