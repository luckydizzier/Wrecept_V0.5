using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Threading.Tasks;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public partial class DateFilterViewModel : ObservableObject
{
    private readonly Func<DateOnly?, DateOnly?, Task> _applyFilter;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private DateOnly? _fromDate;

    [ObservableProperty]
    private DateOnly? _toDate;

    public DateFilterViewModel(Func<DateOnly?, DateOnly?, Task> applyFilter, INavigationService navigation)
    {
        _applyFilter = applyFilter;
        _navigation = navigation;
    }

    [RelayCommand]
    private async Task ApplyAsync()
    {
        await _applyFilter(FromDate, ToDate);
        _navigation.CloseCurrentView();
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigation.CloseCurrentView();
    }
}
