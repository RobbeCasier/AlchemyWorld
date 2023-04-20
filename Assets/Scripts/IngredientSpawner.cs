using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSpawner : MonoBehaviour
{

    [Header("Spawn Setting")]
    [SerializeField] private DropQuality _SpawnQuality;
    [SerializeField] private NaturalIngredient _NaturalIngredient; 

    private void Awake()
    {
        Spawn();
    }
    public void Spawn()
    {
        var gameObject = Instantiate(_NaturalIngredient.gameObject, transform.position, Quaternion.identity);
        gameObject.GetComponent<NaturalIngredient>()._ingredient.Item.SetDropQuality(_SpawnQuality);
    }
}
