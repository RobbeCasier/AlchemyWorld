using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ItemManager : MonoBehaviour
{
    public static ItemManager _instance { get; private set; }
    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Found more than one Item Manager in the scene");
        }
        _instance = this;
    }

    [SerializeField] private XRInteractionManager _InteractiveManager;
    public XRInteractionManager InteractionManager
    {
        get
        {
            return _InteractiveManager;
        }
    }
    [SerializeField] private Cauldron _Cauldron;
    [SerializeField] private PotionMaker _PotionMaker;
    [SerializeField] private BoughtIngredientSpawner _BoughtIngredientSpawner;

    private List<Effect[]> _existingEffects = new List<Effect[]>();

    public void FillPotion(ref PotionItem potion)
    {
        potion.SetColor(_Cauldron.GetColorWater());
        potion.SetEffects(_Cauldron.GetEffects());
        potion.SetQuality(_Cauldron.GetQualityValue());

        _Cauldron.ResetWater();
        if (potion.Effects.Length == 0)
        {
            potion.Name = "Water";
            return;
        }
        else
        {
            uint id = 0;
            if (_existingEffects.Count > 0)
            {
                for (int i = 0; i < _existingEffects.Count; i++)
                {
                    if (_existingEffects[i].Equals(potion.Effects))
                    {
                        potion.SetId(id);
                        return;
                    }
                }
            }
            potion.SetId(id);
            _existingEffects.Add(potion.Effects);
            return;
        }
    }

    public Effect[] GetEffectsFromExistingPotion(uint id)
    {
        return _existingEffects[(int)id];
    }

    public Cauldron GetCauldron()
    {
        return _Cauldron;
    }

    public BoughtIngredientSpawner GetBoughtIngredientSpawner()
    {
        return _BoughtIngredientSpawner;
    }

    //private List<KeyValuePair<StandardItems, int>> _refIngredientList = new List<KeyValuePair<StandardItems, int>>();

    //[SerializeField] private List<Ingredient> _BaseIngredients = new List<Ingredient>();

    //public void InitializeNewGame()
    //{
    //    int maxItems = (int)StandardItems.NONE;
    //    List<NaturalIngredient> unAssignedIngredients = _NaturalIngredientsObjects;
    //    for (int i = 0; i < maxItems; i++)
    //    {
    //        int randomNr;
    //        int nr;
    //        //find random ingredient from the unassigned ingredients
    //        randomNr = UnityEngine.Random.Range(0, unAssignedIngredients.Count - 1);
    //        //find the index from all ingredients
    //        nr = _NaturalIngredientsObjects.FindIndex(x => x.Equals(unAssignedIngredients[randomNr]));
    //        //make ref item
    //        _refIngredientList.Add(new KeyValuePair<StandardItems, int>((StandardItems)i, nr));
    //        //remove the newly assigned ingredient
    //        unAssignedIngredients.RemoveAt(randomNr);
    //    }
    //}

    //[Serializable]
    //private class BaseIngredients
    //{
    //    [SerializeField] Ingredient Ingredient = new Ingredient();
    //}
}
