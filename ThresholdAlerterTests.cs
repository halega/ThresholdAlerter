using System;
using Xunit;

public class ThresholdAlerterTests
{
    [Fact]
    public void Smoke()
    {
        var handler = new ThresholdHandler();
        var handler30 = new ThresholdHandler();

        var alerter = new ThresholdAlerter();
        alerter.Add(10, handler.AlertHandler);
        alerter.Add(20, handler.AlertHandler);
        alerter.Add(30, handler30.AlertHandler);

        alerter.Check(0);
        Assert.Equal(0, handler.CallCount);
        alerter.Check(9);
        Assert.Equal(0, handler.CallCount);
        alerter.Check(10);
        Assert.Equal(1, handler.CallCount);
        Assert.Equal(10, handler.ReachedThreshold);
        alerter.Check(11);
        Assert.Equal(1, handler.CallCount);
        alerter.Check(29);
        Assert.Equal(2, handler.CallCount);
        Assert.Equal(20, handler.ReachedThreshold);
        alerter.Check(100);
        Assert.Equal(2, handler.CallCount);
        Assert.Equal(1, handler30.CallCount);
        Assert.Equal(30, handler30.ReachedThreshold);
        alerter.Check(10);
        Assert.Equal(2, handler.CallCount);
        alerter.Check(21);
        Assert.Equal(3, handler.CallCount);
        Assert.Equal(20, handler.ReachedThreshold);
    }

    private class ThresholdHandler
    {
        public int CallCount { get; private set; }
        public int ReachedThreshold { get; private set; }

        public void AlertHandler(int reachedThreshold)
        {
            CallCount++;
            ReachedThreshold = reachedThreshold;
        } 
    }
}
