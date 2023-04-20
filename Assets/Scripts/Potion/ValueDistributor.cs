//Written by Robbe Casier
using UnityEngine;

//script to distribute any int value over an int amount of instances

//-----------------------------------------------------
//Value = 2000
//MaxDistribution = 6
//normally that would be 333.333...
//but this is possible when 4 times 333 and 2 times 334
//coincidentally 4 = 6-2
//and 2 is 2000 % 6
//-----------------------------------------------------

//this always works

public class ValueDistributor : MonoBehaviour
{
    public int Value { private get; set; }
    public int MaxDistribution { private get; set; }

    private int _firstBatch;
    private int _firstBatchValue;

    private void CalculateDistribution()
    {
        _firstBatch = MaxDistribution - (Value % MaxDistribution);
        _firstBatchValue = Value / MaxDistribution;
    }

    public int GetDistributedValue(uint distributionIndex)
    {
        if (_firstBatch == 0) 
            CalculateDistribution();

        if (distributionIndex < _firstBatch)
        {
            return _firstBatchValue;
        }
        else
            return _firstBatchValue+1;
    }
}
