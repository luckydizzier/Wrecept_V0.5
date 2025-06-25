using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wrecept.ViewModels;

public abstract partial class RestorableListViewModel<T> : ViewModelBase
{
    protected abstract IList<T> Items { get; }

    [ObservableProperty]
    private T? _selectedItem;

    public void RestoreSelection(Guid id)
    {
        var prop = typeof(T).GetProperty("Id");
        if (prop is null) return;
        var item = Items.FirstOrDefault(i => prop.GetValue(i) is Guid g && g == id);
        if (item is not null)
            SelectedItem = item;
    }

    public void SelectFirst()
    {
        if (Items.Count > 0)
            SelectedItem = Items[0];
    }

    public void SelectLast()
    {
        if (Items.Count > 0)
            SelectedItem = Items[^1];
    }

    public virtual T? GetDefaultSelection() => Items.FirstOrDefault();
}
