using Wrecept.Core.Domain;
using Wrecept.Core.Services;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class InlineSupplierCreatorViewModelTests
{
    [Fact]
    public void SaveCommand_ShouldCreateSupplier()
    {
        var repo = new Wrecept.Core.Repositories.InMemorySupplierRepository();
        var service = new DefaultSupplierService(repo);

        var vm = new InlineSupplierCreatorViewModel(service, "Teszt");
        Supplier? created = null;
        vm.Saved += s => created = s;

        vm.SaveCommand.Execute(null);

        Assert.NotNull(created);
        Assert.Equal("Teszt", created!.Name);
    }
}
