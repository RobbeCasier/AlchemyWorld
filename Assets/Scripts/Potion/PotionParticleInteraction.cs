using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionParticleInteraction : MonoBehaviour
{
    [SerializeField] private Potion _Potion;
    [SerializeField] private ValueDistributor _ValueDistributor;
    [SerializeField] private ParticleSystem _WaterStream;
    private uint _currentNrOfCalls = 0;
    private void OnParticleCollision(GameObject other)
    {
        if (other.layer != 4)
            return;

        var amount = _ValueDistributor.GetDistributedValue(_currentNrOfCalls);
        _currentNrOfCalls++;
        ItemManager._instance.GetCauldron().EnterPotion(amount, _Potion._item);
    }
}
