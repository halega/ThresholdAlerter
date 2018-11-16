using System;
using System.Collections.Generic;

public class ThresholdAlerter
{
    private SortedSet<int> thresholds;
    private Dictionary<int, Action<int>> alerts;
    private int lastThreshold;
    private bool started;

    public ThresholdAlerter()
    {
        thresholds = new SortedSet<int>();
        alerts = new Dictionary<int, Action<int>>();
    }

    public void Add(int threshold, Action<int> alert)
    {
        if (started)
        {
            throw new Exception("Can't add a new threshold in a working alerter");
        }
        thresholds.Add(threshold);
        alerts.Add(threshold, alert);
        lastThreshold = thresholds.Min - 1;
    }

    public void Check(int value)
    {
        started = true;
        int threshold = ReachedThreshold(value);
        if (threshold > lastThreshold)
        {
            alerts[threshold](threshold);
        }
        lastThreshold = threshold;
    }

    private int ReachedThreshold(int value)
    {
        if (value < thresholds.Min)
        {
            return thresholds.Min - 1;
        }
        return thresholds.GetViewBetween(thresholds.Min, value).Max;
    }
}

