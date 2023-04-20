using FMODUnity;
using TMPro;
using UnityEngine;

public class BookStore : MonoBehaviour
{
    [SerializeField] private StoreList _BuyList;

    [Header("UI")]
    [SerializeField] private GameObject _LeftPanel;
    [SerializeField] private GameObject _RightPanel;
    [SerializeField] private GameObject _PreviousPageButton;
    [SerializeField] private GameObject _NextPageButton;
    [SerializeField] private TMP_Text _LeftPanelName;
    [SerializeField] private TMP_Text _RightPanelName;
    [SerializeField] private TMP_Text _LeftPanelEffects;
    [SerializeField] private TMP_Text _RightPanelEffects;
    [SerializeField] private TMP_Text _LeftPanelPrice;
    [SerializeField] private TMP_Text _RightPanelPrice;

    [Header("CollisionTriggers")]
    [SerializeField] private GameObject _PreviousPageButtonCollision;
    [SerializeField] private GameObject _NextPageButtonCollision;
    [SerializeField] private GameObject _LeftBuyButtonCollision;
    [SerializeField] private GameObject _RightBuyButtonCollision;

    [Header("Sound")]
    [SerializeField] private EventReference _FlipPageSound;

    private int _currentStoreListIndex = 0;

    private uint _leftPrice = 0;
    private uint _rightPrice = 0;

    private void Awake()
    {
        UpdateDisplays();
    }

    private void UpdateDisplays()
    {
        _PreviousPageButton.SetActive((_currentStoreListIndex == 0) ? false : true);
        _PreviousPageButtonCollision.SetActive((_currentStoreListIndex == 0) ? false : true);
        _NextPageButton.SetActive((_currentStoreListIndex + 2 >= _BuyList._IngredientList.Count) ? false : true);
        _NextPageButtonCollision.SetActive((_currentStoreListIndex + 2 >= _BuyList._IngredientList.Count) ? false : true);

        //Pages
        if (_currentStoreListIndex != _BuyList._IngredientList.Count)
        {
            _LeftPanel.SetActive(true);
            _LeftBuyButtonCollision.SetActive(true);
            UpdateLeftDisplay(_BuyList._IngredientList[_currentStoreListIndex]);
        }
        else
        {
            _LeftPanel.SetActive(false);
            _LeftBuyButtonCollision.SetActive(false);
        }
        if (_currentStoreListIndex + 1 < _BuyList._IngredientList.Count)
        {
            _RightPanel.SetActive(true);
            _RightBuyButtonCollision.SetActive(true);
            UpdateRightDisplay(_BuyList._IngredientList[_currentStoreListIndex+1]);
        }
        else
        {
            _RightPanel.SetActive(false);
            _RightBuyButtonCollision.SetActive(false);
        }
    }

    private void UpdateLeftDisplay(NaturalIngredient naturalIngredient)
    {
        IngredientItem ingredient = naturalIngredient._ingredient.Item;
        _LeftPanelName.text = ingredient.Name;
        _LeftPanelEffects.text = "";

        var pricing = General.Instance.Pricing;
        uint price = 0;

        foreach (var effect in ingredient.Effects)
        {
            _LeftPanelEffects.text += effect.GetEffectName() + "<br>";

            switch (effect.Type)
            {
                case EffectType.HEALTH:
                    price += (uint)(pricing.GetEffectBuyPrice(effect.Type, (int)effect.GetHealthType()));
                    break;
                case EffectType.MANA:
                    price += (uint)(pricing.GetEffectBuyPrice(effect.Type, (int)effect.GetManaType()));
                    break;
                case EffectType.BUFF:
                    price += (uint)(pricing.GetEffectBuyPrice(effect.Type, (int)effect.GetBuffType()));
                    break;
                case EffectType.DEBUFF:
                    price += (uint)(pricing.GetEffectBuyPrice(effect.Type, (int)effect.GetDebuffType()));
                    break;
                default:
                    break;
            }
        }
        _leftPrice = price;
        _LeftPanelPrice.text = _leftPrice.ToString() + " GOLD";
    }

    private void UpdateRightDisplay(NaturalIngredient naturalIngredient)
    {
        IngredientItem ingredient = naturalIngredient._ingredient.Item;
        _RightPanelName.text = ingredient.Name;
        _RightPanelEffects.text = "";

        var pricing = General.Instance.Pricing;
        uint price = 0;

        foreach (var effect in ingredient.Effects)
        {
            _RightPanelEffects.text += effect.GetEffectName() + "<br>";

            switch (effect.Type)
            {
                case EffectType.HEALTH:
                    price += (uint)(pricing.GetEffectBuyPrice(effect.Type, (int)effect.GetHealthType()));
                    break;
                case EffectType.MANA:
                    price += (uint)(pricing.GetEffectBuyPrice(effect.Type, (int)effect.GetManaType()));
                    break;
                case EffectType.BUFF:
                    price += (uint)(pricing.GetEffectBuyPrice(effect.Type, (int)effect.GetBuffType()));
                    break;
                case EffectType.DEBUFF:
                    price += (uint)(pricing.GetEffectBuyPrice(effect.Type, (int)effect.GetDebuffType()));
                    break;
                default:
                    break;
            }
        }

        _rightPrice = price;
        _RightPanelPrice.text = _rightPrice.ToString() + " GOLD";
    }

    public void NextPage()
    {
        _currentStoreListIndex = (_currentStoreListIndex + 2 >= _BuyList._IngredientList.Count) ? _currentStoreListIndex : _currentStoreListIndex + 2;
        RuntimeManager.PlayOneShot(_FlipPageSound);
        UpdateDisplays();
    }

    public void PreviousPage()
    {
        _currentStoreListIndex = (_currentStoreListIndex - 2 < 0) ? _currentStoreListIndex : _currentStoreListIndex - 2;
        RuntimeManager.PlayOneShot(_FlipPageSound);
        UpdateDisplays();
    }

    public void Buy(int buyButtonIndx)
    {
        var gold = General.Instance.ActieCharacterStats.Gold;
        switch (buyButtonIndx)
        {
            case 0:
                if (gold < _leftPrice)
                    return;
                General.Instance.ActieCharacterStats.RemoveGold((int)_leftPrice);
                break;
            case 1:
                if (gold < _rightPrice)
                    return;
                General.Instance.ActieCharacterStats.RemoveGold((int)_rightPrice);
                break;
            default:
                break;
        }

        ItemManager._instance.GetBoughtIngredientSpawner().Spawn(_BuyList._IngredientList[_currentStoreListIndex + buyButtonIndx]);
    }
}
