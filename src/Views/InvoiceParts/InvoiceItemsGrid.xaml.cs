using System.Windows;
using System.Windows.Controls;
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


    private void ItemsGrid_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
    {
        if (DataContext is InvoiceItemsViewModel vm && e.Row.Item == vm.Entry)
        {
            if (e.Column.DisplayIndex == 0)
                vm.OpenProductLookup();
            else if (e.Column.DisplayIndex == 2)
                vm.OpenUnitLookup();
            else if (e.Column.DisplayIndex == 4)
                vm.OpenTaxRateLookup();
        }
    }
}
