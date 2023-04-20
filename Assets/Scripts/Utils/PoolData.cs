using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PoolData", menuName = "AlchemyWorld/PoolData", order = 3)]
public class PoolData : ScriptableObject
{
    public string Name;
    public int MaxAmount;
    public GameObject Prefab;
}
