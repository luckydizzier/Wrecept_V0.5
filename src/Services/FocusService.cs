using System.Windows.Controls;
using System.Windows.Threading;
using Wrecept.Infrastructure;
using Wrecept.Views.InvoiceParts;
using Wrecept.Views.Lookup;

namespace Wrecept.Services;

public class FocusService : IFocusService
{
    public void SetInitialFocus(UserControl view)
    {
        switch (view)
        {
            case InvoiceSidebar sidebar:
                if (sidebar.InvoiceList.Items.Count > 0)
                    sidebar.InvoiceList.SelectedIndex = 0;
                Focus(sidebar.InvoiceList);
                break;
            case InvoiceHeader header:
                Focus(header.SupplierNameBox.SearchBox);
                break;
            case InvoiceSummary summary:
                Focus(summary.VatGrid);
                break;
            case LookupDialog dialog:
                Focus(dialog.SearchBox);
                break;
        }
    }

    public void Focus(Control control)
    {
        control.Dispatcher.InvokeAsync(() =>
        {
            if (AppContext.InputLocked)
                return;
            control.Focus();
            if (control is TextBox tb)
                tb.SelectAll();
        }, DispatcherPriority.Background);
    }
}
