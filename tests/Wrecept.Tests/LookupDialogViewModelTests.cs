using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class LookupDialogViewModelTests
{
    [Fact]
    public async Task SearchText_ShouldFilterResults()
    {
        var data = new List<string> { "alma", "banan", "korte" };
        var vm = new LookupDialogViewModel<string>(term => Task.FromResult(data.Where(d => d.Contains(term)).ToList()), s => s);
        await Task.Delay(10); // allow initial load
        vm.SearchText = "a";
        await Task.Delay(10);
        Assert.Contains(vm.Results, i => i.Display == "alma");
        Assert.Contains(vm.Results, i => i.Display == "banan");
        Assert.DoesNotContain(vm.Results, i => i.Display == "korte");
    }
}
