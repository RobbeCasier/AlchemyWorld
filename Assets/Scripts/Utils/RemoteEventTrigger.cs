using System;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.Events;

public class RemoteEventTrigger : MonoBehaviour
{
    [Header("Event without parameter")]
    [SerializeField] bool DefaultEvent;
    [SerializeField] UnityEvent _OnTriggerEnter;
    [SerializeField] UnityEvent _OnTriggerExit;

    [Header("Event with Collider")]
    [SerializeField] bool ColliderEvent;
    [SerializeField] UnityEvent<Collider> _OnTriggerEnterCollider;
    [SerializeField] UnityEvent<Collider> _OnTriggerExitCollider;

    [Header("Event with parameter")]
    [SerializeField] bool ParameterEvent;
    [SerializeField] UnityEvent<object> _OnTriggerEnterParameter;
    [SerializeField] UnityEvent<object> _OnTriggerExitParameter;

    [Header("Extra Options")]
    [SerializeField] bool LayerCheck;
    [SerializeField] LayerMask Layers;

    [SerializeField] bool TagCheck;
    [SerializeField] string TagName;
    

    private void OnTriggerEnter(Collider other)
    {
        if (LayerCheck && !Layers.Contains(other.gameObject.layer))
            return;

        if (TagCheck && !other.gameObject.tag.Equals(TagName))
            return;

        if (DefaultEvent && _OnTriggerEnter != null)
            _OnTriggerEnter.Invoke();

        if (ParameterEvent && _OnTriggerEnterParameter != null)
            _OnTriggerEnterParameter.Invoke(0);

        if (ColliderEvent && _OnTriggerEnterCollider != null)
            _OnTriggerEnterCollider.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (LayerCheck && !Layers.Contains(other.gameObject.layer))
            return;

        if (TagCheck && !other.gameObject.tag.Equals(TagName))
            return;

        if (DefaultEvent && _OnTriggerExit != null)
            _OnTriggerExit.Invoke();

        if (ParameterEvent && _OnTriggerExitParameter != null)
            _OnTriggerExitParameter.Invoke(0);

        if (ColliderEvent && _OnTriggerExitCollider != null)
            _OnTriggerExitCollider.Invoke(other);
    }

    private void OnValidate()
    {
        if (!DefaultEvent)
        {
            _OnTriggerEnter.RemoveAllListeners();
            _OnTriggerExit.RemoveAllListeners();
        }
        if (!ParameterEvent)
        {
            _OnTriggerEnterParameter.RemoveAllListeners();
            _OnTriggerExitParameter.RemoveAllListeners();
        }
        if (!ColliderEvent)
        {
            _OnTriggerEnterCollider.RemoveAllListeners();
            _OnTriggerExitCollider.RemoveAllListeners();
        }
    }
}
