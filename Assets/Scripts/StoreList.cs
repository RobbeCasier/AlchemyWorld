using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Store List", menuName = "AlchemyWorld/Store List")]
public class StoreList : ScriptableObject
{
    public List<NaturalIngredient> _IngredientList;
}
