using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class NaturalIngredient : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable _interactable;
    [SerializeField] private Ingredient _Ingredient;

    public Ingredient _ingredient { get { return _Ingredient; } }

    private void Start()
    {
        _interactable.interactionManager = ItemManager._instance.InteractionManager;
    }
}
