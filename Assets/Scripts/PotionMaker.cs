using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PotionMaker : MonoBehaviour
{
    [SerializeField] private Transform _PotionSpawnLocation;
    [SerializeField] private Transform _CorkSpawnLocation;
    [SerializeField] private GameObject _PotionPrefab;
    [SerializeField] private GameObject _CorkPrefab;
    [SerializeField] private XRSocketInteractor _SpawnSocket;

    bool _canSpawn = true;
    private void Start()
    {
        _SpawnSocket.interactionManager = ItemManager._instance.InteractionManager;
    }

    public void ReleasPotionSocket()
    {
        _canSpawn = true;
    }
    public void OccupyPotionSocket()
    {
        _canSpawn = false;
    }

    public bool SpawnPotions()
    {
        if (!_canSpawn)
            return _canSpawn;

        GameObject gameObject = Instantiate(_PotionPrefab, _PotionSpawnLocation.position, _PotionSpawnLocation.rotation);
        Instantiate(_CorkPrefab, _CorkSpawnLocation.position, _CorkSpawnLocation.rotation);
        PotionItem potion = new PotionItem();
        ItemManager._instance.FillPotion(ref potion);
        potion.Price = InitializePricePotion(potion.Effects);
        gameObject.GetComponent<Potion>().InitializePotion(potion);
        bool spawned = true;
        _canSpawn = false;
        return spawned;
    }

    private uint InitializePricePotion(Effect[] effects)
    {
        var reputationLevel = General.Instance.ActieCharacterStats.ShopReputationLevel;
        var pricing = General.Instance.Pricing;
        uint price = 0;
        float priceIncrease = pricing.ReputationPriceIncrease[reputationLevel - 1];
        if (effects.Length > 1)
            priceIncrease += pricing.PercentageIncreaseAdditionalEffects * effects.Length - 1;
        foreach (Effect effect in effects)
        {
            switch (effect.Type)
            {
                case EffectType.HEALTH:
                    price += (uint)(pricing.GetEffectSellPrice(effect.Type, (int)effect.GetHealthType()));
                    break;
                case EffectType.MANA:
                    price += (uint)(pricing.GetEffectSellPrice(effect.Type, (int)effect.GetManaType()));
                    break;
                case EffectType.BUFF:
                    price += (uint)(pricing.GetEffectSellPrice(effect.Type, (int)effect.GetBuffType()));
                    break;
                case EffectType.DEBUFF:
                    price += (uint)(pricing.GetEffectSellPrice(effect.Type, (int)effect.GetDebuffType()));
                    break;
                default:
                    break;
            }

        }
        price = (uint)(price * priceIncrease);
        return price;
    }
}
