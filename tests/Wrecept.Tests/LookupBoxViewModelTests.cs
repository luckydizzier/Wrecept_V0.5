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

    [Fact]
    public void Accept_ShouldCloseDropdownAndInvokeCallback()
    {
        var selected = string.Empty;
        var vm = new LookupBoxViewModel<string>(_ => Task.FromResult(new List<string> { "x" }), s => s, s => selected = s, () => { });
        vm.Open();
        vm.SelectedItem = new LookupItem<string>("x", "x");

        vm.Accept();

        Assert.Equal("x", selected);
        Assert.False(vm.IsDropDownOpen);
    }

    [Fact]
    public void Cancel_ShouldCloseDropdownAndInvokeCallback()
    {
        var canceled = false;
        var vm = new LookupBoxViewModel<string>(_ => Task.FromResult(new List<string>()), s => s, _ => { }, () => canceled = true);
        vm.Open();

        vm.Cancel();

        Assert.True(canceled);
        Assert.False(vm.IsDropDownOpen);
    }
}
