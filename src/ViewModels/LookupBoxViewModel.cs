using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace Wrecept.ViewModels;

public class LookupItem<T>(T value, string display)
{
    public T Value { get; } = value;
    public string Display { get; } = display;
}

public partial class LookupBoxViewModel<T> : ObservableObject
{
    private readonly Func<string, Task<List<T>>> _searchFunc;
    private readonly Func<T, string> _displaySelector;
    private readonly Action<T> _onSelected;
    private readonly Action _onCanceled;

    [ObservableProperty]
    private ObservableCollection<LookupItem<T>> _results = new();

    [ObservableProperty]
    private LookupItem<T>? _selectedItem;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private bool _isDropDownOpen;

    public LookupBoxViewModel(
        Func<string, Task<List<T>>> searchFunc,
        Func<T, string> displaySelector,
        Action<T> onSelected,
        Action onCanceled)
    {
        _searchFunc = searchFunc;
        _displaySelector = displaySelector;
        _onSelected = onSelected;
        _onCanceled = onCanceled;
        _ = UpdateResultsAsync();
    }

    partial void OnSearchTextChanged(string value) => _ = UpdateResultsAsync();

    private async Task UpdateResultsAsync()
    {
        var list = await _searchFunc(SearchText);
        Results = new ObservableCollection<LookupItem<T>>(list.Select(i => new LookupItem<T>(i, _displaySelector(i))));
        if (Results.Count > 0)
            SelectedItem = Results[0];
    }

    public void Accept()
    {
        if (SelectedItem != null)
            _onSelected(SelectedItem.Value);
        IsDropDownOpen = false;
    }

    public void Cancel()
    {
        _onCanceled();
        IsDropDownOpen = false;
    }

    public void Open()
    {
        IsDropDownOpen = true;
        _ = UpdateResultsAsync();
    }
}
