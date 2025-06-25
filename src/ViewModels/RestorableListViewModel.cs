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

    public void EnsureValidSelection()
    {
        if (SelectedItem is null && Items.Count > 0)
            SelectedItem = Items[^1];
    }

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

    public bool MovePageDown(int pageSize = 10)
    {
        if (Items.Count == 0)
            return false;

        if (SelectedItem is null)
        {
            SelectedItem = Items[0];
            return true;
        }

        var index = Items.IndexOf(SelectedItem);
        if (index >= Items.Count - 1)
            return false;

        var newIndex = Math.Min(index + pageSize, Items.Count - 1);
        SelectedItem = Items[newIndex];
        return true;
    }

    public bool MovePageUp(int pageSize = 10)
    {
        if (Items.Count == 0)
            return false;

        if (SelectedItem is null)
        {
            SelectedItem = Items[0];
            return true;
        }

        var index = Items.IndexOf(SelectedItem);
        if (index <= 0)
            return false;

        var newIndex = Math.Max(index - pageSize, 0);
        SelectedItem = Items[newIndex];
        return true;
    }

    public virtual T? GetDefaultSelection() => Items.FirstOrDefault();
}
