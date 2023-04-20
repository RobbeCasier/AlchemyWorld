using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BookStoreUIInteraction : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Image _PreviousPageImage;
    [SerializeField] private Image _NextPageImage;
    [SerializeField] private Image _LeftBuyImage;
    [SerializeField] private Image _RightBuyImage;

    #region PageButtons
    public void PressPreviousPageButton()
    {
        _PreviousPageImage.color = Color.red;
    }

    public void PressNextPageButton()
    {
        _NextPageImage.color = Color.red;
    }

    public void PressedPreviousPageButton()
    {
        _PreviousPageImage.color = Color.white;
    }

    public void PressedNextPageButton()
    {
        _NextPageImage.color = Color.white;
    }
    #endregion

    #region BuyButtons
    public void PressLeftBuyButton()
    {

    }

    public void PressRightBuyButton()
    {

    }

    public void PressedLeftBuyButton()
    {

    }

    public void PressedRightBuyButton()
    {

    }
    #endregion
}
