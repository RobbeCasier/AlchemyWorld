using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CustomerInitData", menuName = "AlchemyWorld/Customer Init Data")]
public class CustomerInitData : ScriptableObject
{
    [SerializeField] public List<RepData> repData;
}

[Serializable]
public struct RepData
{
    public int MinGold;
    public int MaxGold;

    public int MinEffects;
    public int MaxEffects;

    public int MaxAmountOfPotions;
}