using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;

namespace Wrecept.ViewModels;

public partial class DateFilterViewModel : ObservableObject
{
    private readonly Func<DateOnly?, DateOnly?, Task> _applyFilter;

    [ObservableProperty]
    private DateOnly? _fromDate;

    [ObservableProperty]
    private DateOnly? _toDate;

    public DateFilterViewModel(Func<DateOnly?, DateOnly?, Task> applyFilter)
    {
        _applyFilter = applyFilter;
    }

    [RelayCommand]
    private async Task ApplyAsync(object window)
    {
        await _applyFilter(FromDate, ToDate);
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
