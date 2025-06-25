using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace Wrecept.ViewModels;

public partial class DateFilterViewModel : ObservableObject
{
    private readonly Action<DateOnly?, DateOnly?> _applyFilter;

    [ObservableProperty]
    private DateOnly? _fromDate;

    [ObservableProperty]
    private DateOnly? _toDate;

    public DateFilterViewModel(Action<DateOnly?, DateOnly?> applyFilter)
    {
        _applyFilter = applyFilter;
    }

    [RelayCommand]
    private void Apply(object window)
    {
        _applyFilter(FromDate, ToDate);
        if (window is System.Windows.Window w)
            w.DialogResult = true;
    }

    [RelayCommand]
    private void Cancel(object window)
    {
        if (window is System.Windows.Window w)
            w.DialogResult = false;
    }
}
