using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text _GoldValue;
    [SerializeField] private TMP_Text _ReputationValue;
    [SerializeField] private Slider _ReputationXP;

    private CharacterStats _characterStats = null;

    private void Start()
    {
        _characterStats = General.Instance.ActieCharacterStats;
        _characterStats.GoldUpdate.AddListener(GoldUpdate);
        _characterStats.ReputationUpdate.AddListener(ReputationUpdate);
        _ReputationValue.text = _characterStats.ShopReputationLevel.ToString();
        _GoldValue.text = _characterStats.Gold.ToString();
    }

    private void GoldUpdate()
    {
        _GoldValue.text = _characterStats.Gold.ToString();
    }
    private void ReputationUpdate()
    {
        _ReputationValue.text = _characterStats.ShopReputationLevel.ToString();
        _ReputationXP.value = _characterStats.ShopReputationXP;
    }
}
