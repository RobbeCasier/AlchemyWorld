using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Shelf : MonoBehaviour
{
    [SerializeField] private List<XRSocketInteractor> sockets = new List<XRSocketInteractor>();
    private PotionItem[] items = new PotionItem[15];
    private Potion[] potions = new Potion[15];
    [SerializeField] private Transform targetPoint = null;
    public List<PotionItem> Potions { get { return items.ToList(); } }
    public Transform TargetPoint { get { return targetPoint; } }

    public void SocketCheckEnter(int socketID)
    {
        Debug.Log("Entered: " + socketID);
        var interactable = sockets[socketID].GetOldestInteractableSelected();
        var potion = interactable.transform.gameObject.GetComponent<Potion>();
        potions[socketID] = potion;
        items[socketID] = potion._item;
        Debug.Log("Entered: " + potion.gameObject.name);
    }

    public void SocketCheckExit(int socketID)
    {
        Debug.Log("Exited: " + potions[socketID].name);
        items[socketID] = null;
        potions[socketID] = null;
    }
    public void RemovePotion(PotionItem item)
    {
        var index = Array.IndexOf(items, item);
        items[index] = null;
        potions[index].RemoveSelf();
        potions[index] = null;
    }
}
