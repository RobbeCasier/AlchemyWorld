using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionMakerUIInteraction : MonoBehaviour
{
    [SerializeField] private Image _Button;
    [SerializeField] private GameObject _ErrorMessage;
    [SerializeField] private PotionMaker _PotionMaker;

    public void PressButton()
    {
        _Button.color = Color.red;
    }

    public void PressedButton()
    {
        _Button.color = Color.white;
        if (!_PotionMaker.SpawnPotions())
            _ErrorMessage.SetActive(true);
        else
            _ErrorMessage.SetActive(false);
    }
}
