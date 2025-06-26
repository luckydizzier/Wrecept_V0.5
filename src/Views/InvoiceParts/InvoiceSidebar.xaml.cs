using System.Windows.Controls;

namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceSidebar : UserControl
{
    public InvoiceSidebar()
    {
        InitializeComponent();
    }

    private void InvoiceList_OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Up && InvoiceList.SelectedIndex == 0)
        {
            var confirm = Infrastructure.AppContext.DialogService.ConfirmNewInvoice();
            e.Handled = true;
            if (confirm && DataContext is ViewModels.InvoiceSidebarViewModel vm)
            {
                vm.SelectedInvoice = new Wrecept.Core.Domain.Invoice();
            }
        }
    }
}
