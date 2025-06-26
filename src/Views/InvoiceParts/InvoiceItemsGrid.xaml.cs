using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.ViewModels;

using Wrecept.Services;
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

    private async void ItemsGrid_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is InvoiceItemsViewModel vm)
        {
            if ((e.Key == Key.F2 || (e.Key == Key.L && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))) && ItemsGrid.SelectedIndex == 0)
            {
                if (ItemsGrid.CurrentColumn.DisplayIndex == 0)
                    await vm.OpenProductLookupAsync();
                else if (ItemsGrid.CurrentColumn.DisplayIndex == 2)
                    await vm.OpenUnitLookupAsync();
                else if (ItemsGrid.CurrentColumn.DisplayIndex == 4)
                    await vm.OpenTaxRateLookupAsync();
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter && ItemsGrid.SelectedIndex == 0 && ItemsGrid.CurrentColumn.DisplayIndex == 0)
            {
                if (await vm.TryOpenProductCreatorAsync())
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
                    if (vm.LastAddSuccess)
                        VisualFeedback.FlashSuccess(ItemsGrid);
                    else
                        VisualFeedback.FlashError(ItemsGrid);
                    ItemsGrid.SelectedIndex = 0;
                    ItemsGrid.CurrentCell = new DataGridCellInfo(ItemsGrid.Items[0], ItemsGrid.Columns[0]);
                    ItemsGrid.BeginEdit();
                    e.Handled = true;
                }
            }

            if (e.Key == Key.Escape && ItemsGrid.SelectedIndex == 0)
            {
                vm.Entry.Clear();
                ItemsGrid.CurrentCell = new DataGridCellInfo(ItemsGrid.Items[0], ItemsGrid.Columns[0]);
                ItemsGrid.BeginEdit();
                e.Handled = true;
            }
        }
    }
}
