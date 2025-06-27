using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wrecept.ViewModels;


public partial class LookupDialogViewModel<T> : ObservableObject
{
    private readonly Func<string, Task<List<T>>> _searchFunc;
    private readonly Func<T, string> _displaySelector;

    [ObservableProperty]
    private ObservableCollection<LookupItem<T>> _results = new();

    [ObservableProperty]
    private LookupItem<T>? _selectedItem;

    [ObservableProperty]
    private string _searchText = string.Empty;

    public LookupDialogViewModel(Func<string, Task<List<T>>> searchFunc, Func<T, string> displaySelector)
    {
        _searchFunc = searchFunc;
        _displaySelector = displaySelector;
        _ = UpdateResultsAsync();
    }

    partial void OnSearchTextChanged(string value)
    {
        _ = UpdateResultsAsync();
    }

    private async Task UpdateResultsAsync()
    {
        var list = await _searchFunc(SearchText);
        Results = new ObservableCollection<LookupItem<T>>(list.Select(i => new LookupItem<T>(i, _displaySelector(i))));
    }

    [RelayCommand]
    private void Confirm(object window)
    {
        if (window is Window w)
            w.DialogResult = true;
    }

    [RelayCommand]
    private void Cancel(object window)
    {
        if (window is Window w)
            w.DialogResult = false;
    }
}
