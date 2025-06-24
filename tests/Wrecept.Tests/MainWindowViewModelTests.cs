using System;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class MainWindowViewModelTests
{
    [Fact]
    public async Task LoadInvoicesAsync_ShouldPopulateCollection()
    {
        var repo = new InMemoryInvoiceRepository();
        await repo.AddAsync(new Invoice { Id = Guid.NewGuid(), SerialNumber = "1" });
        var service = new DefaultInvoiceService(repo);
        var vm = new MainWindowViewModel(service);

        await vm.LoadInvoicesCommand.ExecuteAsync(null);

        Assert.Single(vm.Invoices);
    }

    [Fact]
    public async Task AddInvoiceCommand_ShouldAddInvoice()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var vm = new MainWindowViewModel(service);

        await vm.AddInvoiceCommand.ExecuteAsync(null);

        Assert.Single(vm.Invoices);
        Assert.Single(await service.GetAllAsync());
    }

    [Fact]
    public async Task DeleteInvoiceCommand_ShouldRemoveInvoiceAndMaintainSelection()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var vm = new MainWindowViewModel(service);

        await vm.AddInvoiceCommand.ExecuteAsync(null);
        var invoice = vm.SelectedInvoice!;

        await vm.DeleteInvoiceCommand.ExecuteAsync(invoice);

        Assert.Empty(vm.Invoices);
        Assert.Null(vm.SelectedInvoice);
    }

    [Fact]
    public async Task DeleteInvoiceCommand_ShouldSelectLastRemainingInvoice()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var vm = new MainWindowViewModel(service);

        await vm.AddInvoiceCommand.ExecuteAsync(null);
        await vm.AddInvoiceCommand.ExecuteAsync(null);
        var first = vm.Invoices[0];

        await vm.DeleteInvoiceCommand.ExecuteAsync(first);

        Assert.Single(vm.Invoices);
        Assert.Equal(vm.Invoices[0], vm.SelectedInvoice);
    }

    [Fact]
    public void EnsureValidSelection_ShouldSelectLastWhenNull()
    {
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new MainWindowViewModel(service);
        vm.Invoices.Add(new Invoice { Id = Guid.NewGuid(), SerialNumber = "1" });

        vm.SelectedInvoice = null;
        vm.EnsureValidSelection();

        Assert.NotNull(vm.SelectedInvoice);
    }

    [Fact]
    public void MoveSelectionDown_ShouldNotMovePastEnd()
    {
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new MainWindowViewModel(service);
        vm.Invoices.Add(new Invoice { Id = Guid.NewGuid(), SerialNumber = "1" });
        vm.SelectedInvoice = vm.Invoices[0];

        var moved = vm.MoveSelectionDown();

        Assert.False(moved);
        Assert.Equal(vm.Invoices[0], vm.SelectedInvoice);
    }

    [Fact]
    public void MoveSelectionUp_ShouldNotMoveBeforeStart()
    {
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new MainWindowViewModel(service);
        vm.Invoices.Add(new Invoice { Id = Guid.NewGuid(), SerialNumber = "1" });
        vm.SelectedInvoice = vm.Invoices[0];

        var moved = vm.MoveSelectionUp();

        Assert.False(moved);
        Assert.Equal(vm.Invoices[0], vm.SelectedInvoice);
    }

    [Fact]
    public void RapidMoveSelectionUp_ShouldRemainAtTop()
    {
        var service = new DefaultInvoiceService(new InMemoryInvoiceRepository());
        var vm = new MainWindowViewModel(service);
        vm.Invoices.Add(new Invoice { Id = Guid.NewGuid(), SerialNumber = "1" });
        vm.Invoices.Add(new Invoice { Id = Guid.NewGuid(), SerialNumber = "2" });
        vm.SelectedInvoice = vm.Invoices[0];

        for (int i = 0; i < 5; i++)
            vm.MoveSelectionUp();

        Assert.Equal(vm.Invoices[0], vm.SelectedInvoice);
    }
}
