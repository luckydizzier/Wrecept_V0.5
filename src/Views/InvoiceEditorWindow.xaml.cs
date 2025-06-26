namespace Wrecept.Views;

using System.Windows;

public partial class InvoiceEditorWindow : Window
{
    public InvoiceEditorWindow()
    {
        InitializeComponent();
        DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        if (e.OldValue is ViewModels.InvoiceEditorViewModel oldVm)
        {
            oldVm.PropertyChanged -= ViewModelOnPropertyChanged;
        }

        if (e.NewValue is ViewModels.InvoiceEditorViewModel newVm)
        {
            newVm.PropertyChanged += ViewModelOnPropertyChanged;
        }
    }

    private void ViewModelOnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (sender is ViewModels.InvoiceEditorViewModel vm &&
            e.PropertyName == nameof(ViewModels.InvoiceEditorViewModel.ExitRequested) &&
            vm.ExitRequested)
        {
            Close();
        }
    }
}
