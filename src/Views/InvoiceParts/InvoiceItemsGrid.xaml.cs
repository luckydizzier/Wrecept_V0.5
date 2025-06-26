using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.ViewModels;

namespace Wrecept.Views.InvoiceParts;

public partial class InvoiceItemsGrid : UserControl
{
    public InvoiceItemsGrid()
    {
        InitializeComponent();
    }

    private void ItemsGrid_OnLoaded(object sender, RoutedEventArgs e)
    {
        if (ItemsGrid.Items.Count > 0)
        {
            ItemsGrid.SelectedIndex = 0;
            ItemsGrid.CurrentCell = new DataGridCellInfo(ItemsGrid.Items[0], ItemsGrid.Columns[0]);
            ItemsGrid.BeginEdit();
        }
    }

    private void ItemsGrid_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is InvoiceItemsViewModel vm)
        {
            if ((e.Key == Key.F2 || (e.Key == Key.L && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))) && ItemsGrid.SelectedIndex == 0)
            {
                if (ItemsGrid.CurrentColumn.DisplayIndex == 0)
                    vm.OpenProductLookupAsync().GetAwaiter().GetResult();
                else if (ItemsGrid.CurrentColumn.DisplayIndex == 2)
                    vm.OpenUnitLookupAsync().GetAwaiter().GetResult();
                else if (ItemsGrid.CurrentColumn.DisplayIndex == 4)
                    vm.OpenTaxRateLookupAsync().GetAwaiter().GetResult();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter && ItemsGrid.SelectedIndex == 0 && ItemsGrid.CurrentColumn.DisplayIndex == 0)
            {
                if (vm.TryOpenProductCreator())
                {
                    e.Handled = true;
                    return;
                }
            }

            if (e.Key == Key.Enter && ItemsGrid.SelectedIndex == 0 && ItemsGrid.CurrentColumn.DisplayIndex == 4)
            {
                if (vm.AddItemCommand.CanExecute(null))
                {
                    vm.AddItemCommand.Execute(null);
                    ItemsGrid.SelectedIndex = 0;
                    ItemsGrid.CurrentCell = new DataGridCellInfo(ItemsGrid.Items[0], ItemsGrid.Columns[0]);
                    ItemsGrid.BeginEdit();
                    e.Handled = true;
                }
            }
        }
    }
}
