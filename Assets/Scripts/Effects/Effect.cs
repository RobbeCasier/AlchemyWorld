using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
public enum EffectType
{
    HEALTH,
    MANA,
    BUFF,
    DEBUFF
}

public enum HealthType
{
    HEALTH
}

public enum ManaType
{
    MANA
}

public enum BuffType
{
    DEFUP
}

public enum DebuffType
{
    DEFDOWN
}
[Serializable]
public class Effect
{
    [SerializeField] public EffectType Type;

    [SerializeField] private HealthType HealthType;
    [SerializeField] private ManaType ManaType;
    [SerializeField] private BuffType BuffType;
    [SerializeField] private DebuffType DebuffType;

    public HealthType GetHealthType()
    {
        return HealthType;
    }

    public ManaType GetManaType()
    {
        return ManaType;
    }

    public BuffType GetBuffType()
    {
        return BuffType;
    }

    public DebuffType GetDebuffType()
    {
        return DebuffType;
    }

    public string GetEffectName()
    {
        switch (Type)
        {
            case EffectType.HEALTH:
                return GetHealthEffectName(HealthType);
            case EffectType.MANA:
                return GetManaEffectName(ManaType);
            case EffectType.BUFF:
                return GetBuffEffectName(BuffType);
            case EffectType.DEBUFF:
                return GetDebuffEffectName(DebuffType);
            default:
                return "None";
        }
    }

    private static string GetHealthEffectName(HealthType hType)
    {
        return hType switch
        {
            HealthType.HEALTH => "Health",
            _ => "None",
        };
    }

    private static string GetManaEffectName(ManaType mType)
    {
        return mType switch
        {
            ManaType.MANA => "Mana",
            _ => "None",
        };
    }

    private static string GetBuffEffectName(BuffType bType)
    {
        return bType switch
        {
            BuffType.DEFUP => "Def Up",
            _ => "None",
        };
    }

    private static string GetDebuffEffectName(DebuffType dType)
    {
        return dType switch
        {
            DebuffType.DEFDOWN => "Def Down",
            _ => "None",
        };
    }
    public static string GetRandomEffect()
    {
        System.Random random= new System.Random();
        EffectType randomEffectType = (EffectType)Enum.GetValues(typeof(EffectType)).GetValue(random.Next(Enum.GetValues(typeof(EffectType)).Length));
        string effectName = "";
        switch (randomEffectType)
        {
            case EffectType.HEALTH:
                HealthType randomHealthEffect = (HealthType)Enum.GetValues(typeof(HealthType)).GetValue(random.Next(Enum.GetValues(typeof(HealthType)).Length));
                effectName = GetHealthEffectName(randomHealthEffect);
                return effectName;
            case EffectType.MANA:
                ManaType randomManaEffect = (ManaType)Enum.GetValues(typeof(ManaType)).GetValue(random.Next(Enum.GetValues(typeof(ManaType)).Length));
                effectName = GetManaEffectName(randomManaEffect);
                return effectName;
            case EffectType.BUFF:
                BuffType randomBuffEffect = (BuffType)Enum.GetValues(typeof(BuffType)).GetValue(random.Next(Enum.GetValues(typeof(BuffType)).Length));
                effectName = GetBuffEffectName(randomBuffEffect);
                return effectName;
            case EffectType.DEBUFF:
                DebuffType randomDebuffEffect = (DebuffType)Enum.GetValues(typeof(DebuffType)).GetValue(random.Next(Enum.GetValues(typeof(DebuffType)).Length));
                effectName = GetDebuffEffectName(randomDebuffEffect);
                return effectName;
            default:
                break;
        }
        return null;
    }
}
