using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorkToPotion : MonoBehaviour
{
    [SerializeField] private Potion _Potion;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Cork"))
            _Potion._cork = other.gameObject;
    }
}
