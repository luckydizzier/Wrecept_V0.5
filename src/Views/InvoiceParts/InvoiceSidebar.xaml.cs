using System.Windows.Controls;
using Wrecept.Services;

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

    private void InvoiceList_OnPreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Up && InvoiceList.SelectedIndex == 0)
        {
            var confirm = App.Services.GetRequiredService<IKeyboardDialogService>().ConfirmNewInvoice();
            e.Handled = true;
            if (confirm && DataContext is ViewModels.InvoiceSidebarViewModel vm)
            {
                vm.SelectedInvoice = new Wrecept.Core.Domain.Invoice();
            }
        }
    }
}
