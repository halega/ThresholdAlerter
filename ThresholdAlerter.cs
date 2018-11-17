using System;
using System.Collections.Generic;

public class ThresholdAlerter
{
    private SortedSet<int> thresholds;
    private Dictionary<int, Action<int>> alertHandlers;
    private int reachedThreshold;
    private bool started;

    public ThresholdAlerter()
    {
        thresholds = new SortedSet<int>();
        alertHandlers = new Dictionary<int, Action<int>>();
    }

    public void Add(int threshold, Action<int> alertHandler)
    {
        if (started)
        {
            throw new Exception("Can't add a new threshold to a working alerter");
        }
        thresholds.Add(threshold);
        alertHandlers.Add(threshold, alertHandler);
        reachedThreshold = thresholds.Min - 1;
    }

    public void Check(int value)
    {
        started = true;
        int threshold = ReachedThreshold(value);
        if (threshold > reachedThreshold)
        {
            alertHandlers[threshold](threshold);
        }
        reachedThreshold = threshold;
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

