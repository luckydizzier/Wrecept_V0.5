using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.ViewModels;

namespace Wrecept.Views.Lookup;

public partial class LookupBox : UserControl
{
    public LookupBox()
    {
        InitializeComponent();
    }

    private void SearchBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        var vm = DataContext as dynamic;
        vm?.Open();
        if (sender is TextBox tb)
            tb.SelectAll();
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        var vm = DataContext as dynamic;
        if (vm == null) return;
        if (e.Key == Key.Down)
        {
            ResultList.Focus();
            if (ResultList.Items.Count > 0)
                ResultList.SelectedIndex = 0;
            e.Handled = true;
        }
        else if (e.Key == Key.Enter)
        {
            vm.Accept();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            vm.Cancel();
            e.Handled = true;
        }
    }

    private void ResultList_KeyDown(object sender, KeyEventArgs e)
    {
        var vm = DataContext as dynamic;
        if (vm == null) return;
        if (e.Key == Key.Enter)
        {
            vm.Accept();
            e.Handled = true;
        }
        else if (e.Key == Key.Escape)
        {
            vm.Cancel();
            SearchBox.Focus();
            e.Handled = true;
        }
    }
}
