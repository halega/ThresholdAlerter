using System;
using System.Collections.Generic;
using System.Linq;

public class ThresholdAlerterV2
{
    private int[] thresholds;
    public event Action<int> ThresholdReached;
    private int indexOfReachedThreshold;

    public ThresholdAlerterV2(IEnumerable<int> thresholds)
    {
        this.thresholds = thresholds.Distinct().OrderBy(x => x).ToArray();
        indexOfReachedThreshold = -1;
    }

    public void Check(int value)
    {
        int index = IndexOfReachedThreshold(value);
        if (index > indexOfReachedThreshold)
        {
            OnThresholdReached(thresholds[index]);
        }
        indexOfReachedThreshold = index;
    }

    private int IndexOfReachedThreshold(int value)
    {
        int index = -1;
        for (int i = 0; i < thresholds.Length && value >= thresholds[i]; i++)
        {
            index = i;
        }
        return index;
    }

    private void OnThresholdReached(int threshold)
    {
        ThresholdReached?.Invoke(threshold);
    }
}