using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPresencePhysics : MonoBehaviour
{

    public Transform _target;
    public Transform _defaultHand;
    public Transform _collisionHand;
    private Rigidbody _rb;
    private Collider[] _handColliders;
    private bool isGrabbing = false;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _handColliders = GetComponentsInChildren<Collider>();
    }

    private void Awake()
    {
        transform.position = _target.transform.position;
        transform.rotation = _target.transform.rotation;
    }

    public void EnableHandCollider()
    {
        isGrabbing = false;
        foreach (var item in _handColliders)
        {
            item.enabled = true;
        }
    }

    public void EnableHandColliderDelay(float delay)
    {
        Invoke("EnableHandCollider", delay);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collisionHand.gameObject.SetActive(true);
        _defaultHand.gameObject.SetActive(false);
    }

    private void OnCollisionExit(Collision collision)
    {
        _collisionHand.gameObject.SetActive(false);
        _defaultHand.gameObject.SetActive(true);
    }

    public void DisableHandCollider()
    {
        isGrabbing = true;
        foreach (var item in _handColliders)
        {
            item.enabled = false;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isGrabbing && _collisionHand.gameObject.activeSelf)
        {
            //position
            _rb.velocity = (_target.position - transform.position) / Time.fixedDeltaTime;

            //rotation
            Quaternion rotationDifference = _target.rotation * Quaternion.Inverse(transform.rotation);
            rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

            Vector3 rotationDifferenceInDegree = angleInDegree * rotationAxis;

            _rb.angularVelocity = (rotationDifferenceInDegree * Mathf.Deg2Rad / Time.fixedDeltaTime);
        }
        else
        {
            transform.position = _target.transform.position;
            transform.rotation = _target.transform.rotation;
        }
    }
}
