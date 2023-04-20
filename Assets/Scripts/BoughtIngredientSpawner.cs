using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoughtIngredientSpawner : MonoBehaviour
{
    [SerializeField] private Transform _SpawnPoint;

    public void Spawn(NaturalIngredient naturalIngredient)
    {
        var gameObject = Instantiate(naturalIngredient.gameObject, transform.position, Quaternion.identity);
        gameObject.GetComponent<NaturalIngredient>()._ingredient.Item.SetDropQuality(DropQuality.ALL);
    }
}
