using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegister : MonoBehaviour
{
    [SerializeField] private Transform _TargetPoint;
    [SerializeField] private GameObject _Sachet;
    public Transform TargetPoint { get { return _TargetPoint; } }
    public bool IsMoneyOnCounter { get { return _isMoneyOnCounter; } }

    private uint _amountOnCounter = 0;
    private bool _isMoneyOnCounter = false;

    private Vector3 _defaultPouchLocation;

    private void Awake()
    {
        _defaultPouchLocation = _Sachet.transform.position;
    }
    public void AddMoneyOnCounter(uint amount)
    {
        _amountOnCounter += amount;
        _Sachet.SetActive(true);
        _isMoneyOnCounter = true;
    }

    public void CollectGold()
    {
        _Sachet.SetActive(false);
        _Sachet.transform.position = _defaultPouchLocation;
        _Sachet.transform.rotation = Quaternion.Euler(Vector3.zero);

        _isMoneyOnCounter = false;

        var charStats = General.Instance.ActieCharacterStats;
        charStats.AddGold((int)_amountOnCounter);
        charStats.IncreaseReputation(_amountOnCounter);

        _amountOnCounter = 0;
    }
}
