using System.Windows.Controls;
using System.Windows.Input;
using Wrecept.ViewModels;

namespace Wrecept.Views.Inline;

public partial class InlineProductCreator : UserControl
{
    public InlineProductCreator()
    {
        InitializeComponent();
    }

    private void UserControl_KeyDown(object sender, KeyEventArgs e)
    {
        if (DataContext is InlineProductCreatorViewModel vm)
        {
            if (e.Key == Key.Enter)
            {
                if (vm.SaveCommand.CanExecute(null))
                    vm.SaveCommand.Execute(null);
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                if (vm.CancelCommand.CanExecute(null))
                    vm.CancelCommand.Execute(null);
                e.Handled = true;
            }
        }
    }
}
