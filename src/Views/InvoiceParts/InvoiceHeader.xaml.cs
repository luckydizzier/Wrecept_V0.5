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

    private async void SupplierNameBox_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is InvoiceHeaderViewModel vm)
        {
            if ((e.Key == Key.F2 || (e.Key == Key.L && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))))
            {
                await vm.OpenSupplierLookupAsync();
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                if (await vm.TryOpenSupplierCreatorAsync())
                    e.Handled = true;
            }
        }
    }
}
