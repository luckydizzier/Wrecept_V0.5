using System.Collections.Generic;
using Wrecept.Services;
using Xunit;

namespace Wrecept.Tests;

public class FeedbackServiceTests
{
    [Fact]
    public void Accept_ShouldPlaySequence()
    {
        var list = new List<int>();
        var service = new FeedbackService((f,d) => list.Add(f));

        service.Accept();

        Assert.Equal(new[]{800,1000}, list);
    }

    [Fact]
    public void Error_ShouldPlaySequence()
    {
        var list = new List<int>();
        var service = new FeedbackService((f,d) => list.Add(f));

        service.Error();

        Assert.Equal(new[]{500,500}, list);
    }
}
