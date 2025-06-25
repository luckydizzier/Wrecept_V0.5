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

    [Fact]
    public void MovePageDown_ShouldAdvanceByPageSize()
    {
        var vm = new DummyVm();
        for (int i = 0; i < 15; i++)
            vm.List.Add(new Invoice { Id = Guid.NewGuid() });
        vm.SelectedInvoice = vm.List[0];

        var moved = vm.MovePageDown(10);

        Assert.True(moved);
        Assert.Equal(vm.List[10], vm.SelectedInvoice);
    }

    [Fact]
    public void MovePageUp_ShouldRetreatByPageSize()
    {
        var vm = new DummyVm();
        for (int i = 0; i < 15; i++)
            vm.List.Add(new Invoice { Id = Guid.NewGuid() });
        vm.SelectedInvoice = vm.List[14];

        var moved = vm.MovePageUp(10);

        Assert.True(moved);
        Assert.Equal(vm.List[4], vm.SelectedInvoice);
    }

    [Fact]
    public void MovePageDown_ShouldNotMovePastEnd()
    {
        var vm = new DummyVm();
        for (int i = 0; i < 5; i++)
            vm.List.Add(new Invoice { Id = Guid.NewGuid() });
        vm.SelectedInvoice = vm.List[4];

        var moved = vm.MovePageDown(10);

        Assert.False(moved);
        Assert.Equal(vm.List[4], vm.SelectedInvoice);
    }

    [Fact]
    public void MovePageUp_ShouldNotMoveBeforeStart()
    {
        var vm = new DummyVm();
        for (int i = 0; i < 5; i++)
            vm.List.Add(new Invoice { Id = Guid.NewGuid() });
        vm.SelectedInvoice = vm.List[0];

        var moved = vm.MovePageUp(10);

        Assert.False(moved);
        Assert.Equal(vm.List[0], vm.SelectedInvoice);
    }
}
