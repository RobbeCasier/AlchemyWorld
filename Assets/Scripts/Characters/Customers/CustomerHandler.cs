using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{
    private List<Customer> _customerList = new List<Customer>();
    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;
        foreach (var customer in _customerList)
        {
            customer.UpdateCustomer(deltaTime);
        }
    }

    private void SpawnCustomer()
    {
        var customer = new Customer();
    }
}
