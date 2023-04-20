using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ExpSetting", menuName ="AlchemyWorld/Exp Setting")]
public class ExpSettings : ScriptableObject
{
    public int XPUntillNextLv = 1000;
    public List<float> PercentageOfIncomeFromLevel;
}
