using System;
using System.Collections.ObjectModel;
using Wrecept.Core.Domain;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class RestorableListViewModelTests
{
    private class DummyVm : RestorableListViewModel<Invoice>
    {
        public ObservableCollection<Invoice> List { get; } = new();
        protected override IList<Invoice> Items => List;
        public Invoice? SelectedInvoice
        {
            get => SelectedItem;
            set => SelectedItem = value;
        }
    }

    [Fact]
    public void RestoreSelection_ShouldSelectItemWithMatchingId()
    {
        var vm = new DummyVm();
        var a = new Invoice { Id = Guid.NewGuid() };
        var b = new Invoice { Id = Guid.NewGuid() };
        vm.List.Add(a);
        vm.List.Add(b);

        vm.RestoreSelection(b.Id);

        Assert.Equal(b, vm.SelectedInvoice);
    }

    [Fact]
    public void EnsureValidSelection_ShouldSelectLastWhenNull()
    {
        var vm = new DummyVm();
        var a = new Invoice { Id = Guid.NewGuid() };
        var b = new Invoice { Id = Guid.NewGuid() };
        vm.List.Add(a);
        vm.List.Add(b);

        vm.SelectedInvoice = null;
        vm.EnsureValidSelection();

        Assert.Equal(b, vm.SelectedInvoice);
    }
}
