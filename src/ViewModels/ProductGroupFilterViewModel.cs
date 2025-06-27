using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.Services;

namespace Wrecept.ViewModels;

public partial class ProductGroupFilterViewModel : ObservableObject
{
    private readonly Func<Guid?, Task> _apply;
    private readonly IProductGroupService _service;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private List<ProductGroup> _groups = new();

    [ObservableProperty]
    private ProductGroup? _selectedGroup;

    public ProductGroupFilterViewModel(Func<Guid?, Task> apply, IProductGroupService service, INavigationService navigation)
    {
        _apply = apply;
        _service = service;
        _navigation = navigation;
        _ = LoadGroupsAsync();
    }

    private async Task LoadGroupsAsync()
    {
        Groups = await _service.GetAllAsync();
        SelectedGroup = Groups.FirstOrDefault();
    }

    [RelayCommand]
    private async Task ApplyAsync()
    {
        await _apply(SelectedGroup?.Id);
        _navigation.CloseCurrentView();
    }

    [RelayCommand]
    private void Cancel()
    {
        _navigation.CloseCurrentView();
    }
}
