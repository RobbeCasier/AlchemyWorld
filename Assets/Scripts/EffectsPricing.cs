using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pricing", menuName = "AlchemyWorld/Pricing")]
public class EffectsPricing : ScriptableObject
{
    public List<EffectPrice> EffectsPrices;
    public List<float> ReputationPriceIncrease;
    public float PercentageIncreaseAdditionalEffects;

    public uint GetEffectBuyPrice(EffectType type, int enumTypeId)
    {
        var effectPrice = GetEffectPrice(type, enumTypeId);
        if (effectPrice == null)
            return 0;
        return effectPrice.DefaultBuyPrice;
    }

    public uint GetEffectSellPrice(EffectType type, int enumTypeId)
    {
        var effectPrice = GetEffectPrice(type, enumTypeId);
        if (effectPrice == null)
            return 0;
        Debug.Log("Pricing after check");
        return effectPrice.DefaultSellPrice;
    }

    private EffectPrice GetEffectPrice(EffectType type, int enumTypeId)
    {
        EffectPrice effectPrice = null;
        switch (type)
        {
            case EffectType.HEALTH:
                effectPrice = EffectsPrices.Find(x => x.EffectType.Equals(type) && (int)x.HealthType == enumTypeId);
                break;
            case EffectType.MANA:
                effectPrice = EffectsPrices.Find(x => x.EffectType.Equals(type) && (int)x.ManaType == enumTypeId);
                break;
            case EffectType.BUFF:
                effectPrice = EffectsPrices.Find(x => x.EffectType.Equals(type) && (int)x.BuffType == enumTypeId);
                break;
            case EffectType.DEBUFF:
                effectPrice = EffectsPrices.Find(x => x.EffectType.Equals(type) && (int)x.DebuffType == enumTypeId);
                break;
            default:
                break;
        }
        return effectPrice;
    }
}
