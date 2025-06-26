using System.Windows.Controls;

namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceSummary : UserControl
{
    public InvoiceSummary()
    {
        InitializeComponent();
        Loaded += (_, _) => VatGrid.Focus();
    }
}
