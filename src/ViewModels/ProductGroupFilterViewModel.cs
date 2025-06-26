using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Services;

namespace Wrecept.ViewModels;

public partial class ProductGroupFilterViewModel : ObservableObject
{
    private readonly Func<Guid?, Task> _apply;
    private readonly IProductGroupService _service;

    [ObservableProperty]
    private List<ProductGroup> _groups = new();

    [ObservableProperty]
    private ProductGroup? _selectedGroup;

    public ProductGroupFilterViewModel(Func<Guid?, Task> apply, IProductGroupService service)
    {
        _apply = apply;
        _service = service;
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
        Infrastructure.AppContext.NavigationService.CloseCurrentView();
    }

    [RelayCommand]
    private void Cancel()
    {
        Infrastructure.AppContext.NavigationService.CloseCurrentView();
    }
}
