using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "AlchemyWorld/Ingredient", order = 1)]

public class Ingredient : ScriptableObject
{
    [SerializeField] public IngredientItem Item;
}
