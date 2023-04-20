using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Cork : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable _GrabInteractible;
    private void Start()
    {
        _GrabInteractible.interactionManager = ItemManager._instance.InteractionManager;
    }
}
