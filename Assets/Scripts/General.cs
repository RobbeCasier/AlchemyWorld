using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour
{
    private static General _instance = null;
    public static General Instance 
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }
            return null;
        }
    }
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Found more than one General in the scene");
        }
        _instance = this;
    }


    [SerializeField] private CharacterStats _ActiveCharacterStats;
    public CharacterStats ActieCharacterStats { get { return _ActiveCharacterStats; } }

    [SerializeField] private CustomerInitData _CustomerInitData;
    public CustomerInitData CustomerInitData { get { return _CustomerInitData; } }
    [SerializeField] private ExpSettings _ExpSetting;
    public ExpSettings ExpSetting { get { return _ExpSetting; } }

    [SerializeField] private TimeCicle _Time;
    public TimeCicle GameTime { get { return _Time; } }

    [SerializeField] private CashRegister _CashRegister;
    public CashRegister CashRegister { get { return _CashRegister; }}

    [SerializeField] private List<Shelf> _PotionShelfs;
    public List<Shelf> PotionShelfs { get { return _PotionShelfs; } }

    [SerializeField] private Transform _LeavePoint;
    public Transform LeavePoint { get { return _LeavePoint; } }

    [SerializeField] private EffectsPricing _Pricing;
    public EffectsPricing Pricing { get { return _Pricing; } }
}
