using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawner : MonoBehaviour
{
    [SerializeField] private float _StartHour;
    [SerializeField] private float _EndHour;
    [SerializeField] private float _SpawnRateInHours = 1;

    [SerializeField] private int _maxCustomersADay = 8;
    [SerializeField] private int _maxCustomersPreSpawn = 8;
    [SerializeField] private GameObject _CustomerPrefab;

    private int _currentMaxCustomers = 0;
    private float _timePassed;
    private int _totalSpawnedTimes = 0;
    private int _totalUnspawnedTimes = 0;
    private int _currentCustomerInside = 0;
    private List<Customer> _customers = new List<Customer>();

    private void Awake()
    {
        for (int i = 0; i < _maxCustomersPreSpawn; i++)
        {
            var customer = Instantiate(_CustomerPrefab, transform);
            customer.SetActive(false);
            _customers.Add(customer.GetComponent<Customer>());
        }

        _timePassed = _StartHour - 1;
        _currentMaxCustomers = Random.Range(_maxCustomersADay/2, _maxCustomersADay + 1);
        General.Instance.GameTime.AddListenerToNewDay(NewDayInitialize);
    }

    private void NewDayInitialize()
    {
        _currentMaxCustomers = Random.Range(_maxCustomersADay/2, _maxCustomersADay + 1);
        _totalUnspawnedTimes = 0;
        _totalSpawnedTimes = 0;
        _timePassed= _StartHour - 1;
    }

    private void Update()
    {
        float currentHour = General.Instance.GameTime.GetCurrentHour();
        UpdateCustomerSpawn(currentHour);
    }

    private void UpdateCustomerSpawn(float currentHour)
    {
        if (currentHour >= _StartHour && currentHour < _EndHour)
        {
            float timeDifferenceFromStart = currentHour - _timePassed;
            if (timeDifferenceFromStart >= _SpawnRateInHours)
            {
                _timePassed += _SpawnRateInHours;

                //is a customer inside, don't spawn
                if (_customers[_currentCustomerInside].gameObject.activeInHierarchy)
                {
                    _totalUnspawnedTimes++;
                    return;
                }

                if (_totalSpawnedTimes == _currentMaxCustomers)
                    return;

                //possible to skip a spawn trigger
                if (_currentMaxCustomers + _totalUnspawnedTimes < _maxCustomersADay)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        _totalUnspawnedTimes++;
                        return;
                    }
                }

                _totalSpawnedTimes++;

                //new customer
                _currentCustomerInside = Random.Range(0, _customers.Count);
                _customers[_currentCustomerInside].transform.position = transform.position;
                _customers[_currentCustomerInside].gameObject.SetActive(true);


            }
        }
        else if (currentHour > _EndHour)
        {
            _totalUnspawnedTimes = 0;
            _totalSpawnedTimes = 0;
        }
    }
}
