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
    private async Task ApplyAsync()
    {
        await _applyFilter(FromDate, ToDate);
        Infrastructure.AppContext.NavigationService.CloseCurrentView();
    }

    [RelayCommand]
    private void Cancel()
    {
        Infrastructure.AppContext.NavigationService.CloseCurrentView();
    }
}
