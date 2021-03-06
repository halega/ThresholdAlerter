using System;
using Xunit;

public class ThresholdAlerterTests
{
	[Fact]
	public void Smoke()
	{
		bool alerted = false;
		var alerter = new ThresholdAlerter();
		alerter.Add(10, x =>
		{
			alerted = true;
			Assert.Equal(10, x);
		});
		alerter.Check(10);
		Assert.True(alerted);
	}

	[Fact]
	public void AlertWhenReachedThreshold()
	{
		var handler = new HandlerMock();
		var alerter = new ThresholdAlerter();
		alerter.Add(10, handler.AlertHandler);

		alerter.Check(10);
		Assert.Equal(1, handler.CallCount);
		Assert.Equal(10, handler.ReachedThreshold);
	}

	[Fact]
	public void AlertOnceWhenPassedSeveralThresholds()
	{
		var handler = new HandlerMock();
		var alerter = new ThresholdAlerter();
		alerter.Add(10, handler.AlertHandler);
		alerter.Add(20, handler.AlertHandler);

		alerter.Check(30);
		Assert.Equal(1, handler.CallCount);
		Assert.Equal(20, handler.ReachedThreshold);
	}

	[Fact]
	public void NoAlertWhenNoReachedThreshold()
	{
		var handler = new HandlerMock();
		var alerter = new ThresholdAlerter();
		alerter.Add(100, handler.AlertHandler);

		alerter.Check(0);
		alerter.Check(50);
		alerter.Check(99);
		Assert.Equal(0, handler.CallCount);
	}

	[Fact]
	public void AlertOnceOnReachedThreshold()
	{
		var handler = new HandlerMock();
		var alerter = new ThresholdAlerter();
		alerter.Add(10, handler.AlertHandler);

		// Alert
		alerter.Check(10);
		Assert.Equal(1, handler.CallCount);
		// No alert
		alerter.Check(11);
		Assert.Equal(1, handler.CallCount);
	}

	[Fact]
	public void AlertWhenValueGoesDownAndUpThreshold()
	{
		var handler = new HandlerMock();
		var alerter = new ThresholdAlerter();
		alerter.Add(10, handler.AlertHandler);
		alerter.Add(20, handler.AlertHandler);

		// Alert
		alerter.Check(20);
		Assert.Equal(1, handler.CallCount);
		Assert.Equal(20, handler.ReachedThreshold);
		// No alert
		alerter.Check(0);
		Assert.Equal(1, handler.CallCount);
		// Alert again
		alerter.Check(17);
		Assert.Equal(2, handler.CallCount);
		Assert.Equal(10, handler.ReachedThreshold);
	}

	[Fact]
	public void NoThresholds()
	{
		var alerter = new ThresholdAlerter();
		// No exceptions
		alerter.Check(0);
	}

	[Theory]
	// Implementation limit: threshold can't be int.MinValue
	// [InlineData(int.MinValue, int.MinValue, 1)]
	// [InlineData(int.MaxValue, int.MinValue, 1)]
	[InlineData(int.MinValue, int.MinValue+1, 0)]
	[InlineData(int.MinValue, 0, 0)]
	[InlineData(int.MaxValue, int.MaxValue, 1)]
	[InlineData(int.MaxValue, 0, 1)]
	[InlineData(int.MaxValue, int.MinValue+1, 1)]
	[InlineData(0, 0, 1)]
	[InlineData(-1, -1, 1)]
	public void BoundValues(int value, int threshold, int callCount)
	{
		var handler = new HandlerMock();
		var alerter = new ThresholdAlerter();
		alerter.Add(threshold, handler.AlertHandler);
		alerter.Check(value);
		Assert.Equal(callCount, handler.CallCount);
	}

    [Fact]
    public void ManyChecks()
    {
        var alerter = new ThresholdAlerter();
        var handler = new HandlerMock();
        for (int i = 0; i < 1000; i += 10)
        {
            alerter.Add(i, handler.AlertHandler);
        }
        for (int i = 0; i < 10000; i++)
        {
            alerter.Check(i);
        }
        Assert.Equal(100, handler.CallCount);
        Assert.Equal(990, handler.ReachedThreshold);
    }
	
    private class HandlerMock
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
