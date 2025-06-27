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
        if (DataContext is LookupBoxViewModel<object> vm)
            vm.Open();
    }

    private void SearchBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is not dynamic vm) return;
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
        if (DataContext is not dynamic vm) return;
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
