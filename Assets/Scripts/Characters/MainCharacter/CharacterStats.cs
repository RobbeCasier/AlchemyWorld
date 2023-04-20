using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterStats : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private EventReference _CashIn;
    [SerializeField] private EventReference _CashOut;

    private static int _shopReputation = 1;
    private static int _gold = 50;
    private static int _currentExp = 0;
    private static float _currentShopXPFloat = 0;

    [Header("Events")]
    public UnityEvent GoldUpdate;
    public UnityEvent ReputationUpdate;

    public int ShopReputationLevel
    {
        get
        {
            //value 1-10
            return _shopReputation;
        }
    }

    public int Gold
    {
        get
        {
            return _gold;
        }
    }

    public float ShopReputationXP
    {
        get
        {
            return _currentShopXPFloat;
        }
    }

    public void IncreaseReputation(uint gold)
    {
        var expSetting = General.Instance.ExpSetting;
        float percentage = expSetting.PercentageOfIncomeFromLevel[ShopReputationLevel - 1];
        _currentExp += (int)(percentage * gold);
        if (_shopReputation < 10 && _currentExp > expSetting.XPUntillNextLv)
        {
            _currentExp = expSetting.XPUntillNextLv - _currentExp;
            _shopReputation++;
        }
        _currentShopXPFloat = (float)_currentExp / (float)expSetting.XPUntillNextLv;
        ReputationUpdate.Invoke();
    }

    public void AddGold(int goldAmount)
    {
        _gold += goldAmount;
        GoldUpdate.Invoke();

        RuntimeManager.PlayOneShot(_CashIn);
    }

    public void RemoveGold(int goldAmount)
    {
        _gold -= goldAmount;
        GoldUpdate.Invoke();
        RuntimeManager.PlayOneShot(_CashOut);
    }
}
