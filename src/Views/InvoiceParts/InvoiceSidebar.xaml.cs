using System.Windows.Controls;


namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceSidebar : UserControl
{
    public InvoiceSidebar()
    {
        InitializeComponent();
        Loaded += (_, _) =>
        {
            if (InvoiceList.Items.Count > 0)
                InvoiceList.SelectedIndex = 0;
            InvoiceList.Focus();
        };
    }

}
