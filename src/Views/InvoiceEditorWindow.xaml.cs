namespace Wrecept.Views;

using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using Wrecept.Services;

public partial class InvoiceEditorWindow : UserControl
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
        if (sender is ViewModels.InvoiceEditorViewModel vm)
        {
            if (e.PropertyName == nameof(ViewModels.InvoiceEditorViewModel.ExitRequested) && vm.ExitRequested)
            {
                App.Services.GetRequiredService<INavigationService>().CloseCurrentView();
            }
            else if (e.PropertyName == nameof(ViewModels.InvoiceEditorViewModel.LastSaveSuccess))
            {
                if (vm.LastSaveSuccess)
                    VisualFeedback.FlashSuccess(SaveButton);
                else
                    VisualFeedback.FlashError(SaveButton);
            }
        }
    }
}
