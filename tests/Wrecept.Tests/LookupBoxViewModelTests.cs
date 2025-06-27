using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wrecept.ViewModels;
using Xunit;

namespace Wrecept.Tests;

public class LookupBoxViewModelTests
{
    [Fact]
    public async Task SearchText_ShouldFilterResults()
    {
        var data = new List<string> { "alma", "banan", "korte" };
        var vm = new LookupBoxViewModel<string>(term => Task.FromResult(data.Where(d => d.Contains(term)).ToList()), s => s, _ => { }, () => { });
        vm.Open();
        await Task.Delay(10); // allow initial load
        vm.SearchText = "a";
        await Task.Delay(10);
        Assert.Contains(vm.Results, i => i.Display == "alma");
        Assert.Contains(vm.Results, i => i.Display == "banan");
        Assert.DoesNotContain(vm.Results, i => i.Display == "korte");
    }
}
