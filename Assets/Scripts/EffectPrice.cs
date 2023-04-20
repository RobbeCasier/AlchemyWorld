using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectPrice", menuName = "AlchemyWorld/Effect Price")]
public class EffectPrice : ScriptableObject
{
    public EffectType EffectType;
    public HealthType HealthType;
    public ManaType ManaType;
    public BuffType BuffType;
    public DebuffType DebuffType;

    public uint DefaultBuyPrice;
    public uint DefaultSellPrice;
}
