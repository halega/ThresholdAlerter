using System;
using System.Collections.Generic;
using System.Linq;

public class ThresholdAlerterV2
{
    private int[] thresholds;
    private int indexOfReachedThreshold;
    
    public event Action<int> ThresholdReached;

    public ThresholdAlerterV2(IEnumerable<int> thresholds)
    {
        this.thresholds = thresholds.Distinct().OrderBy(x => x).ToArray();
        indexOfReachedThreshold = -1;
    }

    public void Check(int value)
    {
        int index = IndexOfMaxReachedThreshold(value);
        if (index > indexOfReachedThreshold)
        {
            OnThresholdReached(thresholds[index]);
        }
        indexOfReachedThreshold = index;
    }

    private int IndexOfMaxReachedThreshold(int value)
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