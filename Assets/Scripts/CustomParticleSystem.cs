using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomParticleSystem : MonoBehaviour
{
    public List<GameObject> PooledObjects;
    [SerializeField] private GameObject _ObjectToPool;
    [SerializeField] private int _AmountToPool;
    [SerializeField] private float _MaxLifeTime;

    private Dictionary<GameObject, float> _pooledObjects = new Dictionary<GameObject, float>();

    private void Start()
    {
        PooledObjects = new List<GameObject>();
        GameObject temp;
        for (int i = 0; i < _AmountToPool; i++)
        {
            temp = Instantiate(_ObjectToPool);
            temp.SetActive(false);
            PooledObjects.Add(temp);
        }
    }

    private void FixedUpdate()
    {
        if (_pooledObjects.Count == 0)
            return;

        List<int> remove= new List<int>();
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            var time = _pooledObjects[_pooledObjects.ElementAt(i).Key] += Time.fixedDeltaTime;
            if (time >= _MaxLifeTime)
                remove.Add(i);
        }

        foreach (var index in remove)
        {
            var particle = _pooledObjects.ElementAt(index).Key;
            _pooledObjects.Remove(particle);
            particle.SetActive(false);
        }
    }

    public GameObject SpawnObject()
    {
        for (int i = 0; i < _AmountToPool; i++)
        {
            if (!PooledObjects[i].activeInHierarchy)
            {
                //a chance where the object is not active but still in the list
                if (_pooledObjects.ContainsKey(PooledObjects[i]))
                    _pooledObjects.Remove(PooledObjects[i]);

                _pooledObjects.Add(PooledObjects[i], 0.0f);
                PooledObjects[i].transform.position = transform.position;
                PooledObjects[i].SetActive(true);
                return PooledObjects[i];
            }
        }
        return null;
    }
}
