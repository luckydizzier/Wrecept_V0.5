using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wrecept.Core.Domain;
using Wrecept.Core.Repositories;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Wrecept.Services;
using Xunit;

namespace Wrecept.Tests;

public class InvoiceEditorValidationTests
{
    [Fact]
    public async Task SaveAsync_ShouldNotPersist_WhenFieldsMissing()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var invoice = new Invoice
        {
            SerialNumber = string.Empty,
            Supplier = new Supplier(),
            PaymentMethod = new PaymentMethod(),
        };
        var vm = new InvoiceEditorViewModel(
            invoice,
            true,
            service,
            new DefaultSupplierService(new InMemorySupplierRepository()),
            new DefaultPaymentMethodService(new InMemoryPaymentMethodRepository()),
            new DefaultProductService(new InMemoryProductRepository()),
            new DefaultProductGroupService(new InMemoryProductGroupRepository()),
            new DefaultUnitService(new InMemoryUnitRepository()),
            new DefaultTaxRateService(new InMemoryTaxRateRepository()),
            new JsonPriceHistoryService(),
            new FeedbackService(),
            true);

        await vm.SaveAsync();

        Assert.False(vm.LastSaveSuccess);
        Assert.False(vm.ExitRequested);
        Assert.Empty(await repo.GetAllAsync());
    }

    [Fact]
    public async Task SaveAsync_ShouldPersist_WhenInvoiceValid()
    {
        var repo = new InMemoryInvoiceRepository();
        var service = new DefaultInvoiceService(repo);
        var invoice = new Invoice
        {
            SerialNumber = "1",
            Supplier = new Supplier { Name = "A" },
            PaymentMethod = new PaymentMethod { Label = "Cash" },
        };
        invoice.Items.Add(new InvoiceItem
        {
            Product = new Product { Name = "P" },
            Quantity = 1,
            Unit = new Unit { Name = "db" },
            UnitPriceNet = 100m,
            VatRatePercent = 27m
        });

        var vm = new InvoiceEditorViewModel(
            invoice,
            true,
            service,
            new DefaultSupplierService(new InMemorySupplierRepository()),
            new DefaultPaymentMethodService(new InMemoryPaymentMethodRepository()),
            new DefaultProductService(new InMemoryProductRepository()),
            new DefaultProductGroupService(new InMemoryProductGroupRepository()),
            new DefaultUnitService(new InMemoryUnitRepository()),
            new DefaultTaxRateService(new InMemoryTaxRateRepository()),
            new JsonPriceHistoryService(),
            new FeedbackService(),
            true);

        await vm.SaveAsync();

        Assert.True(vm.LastSaveSuccess);
        Assert.True(vm.ExitRequested);
        Assert.Single(await repo.GetAllAsync());
    }
}
