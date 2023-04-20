using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolSingle : MonoBehaviour
{
    public List<GameObject> PooledObjects;
    [SerializeField] private GameObject _ObjectToPool;
    [SerializeField] private int _AmountToPool;

    private void Start()
    {
        PooledObjects= new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < _AmountToPool; i++)
        {
            temp = Instantiate(_ObjectToPool, transform);
            temp.SetActive(false);
            PooledObjects.Add(temp);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _AmountToPool; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                return PooledObjects[i];
            }
        }
        return null;
    }
}
