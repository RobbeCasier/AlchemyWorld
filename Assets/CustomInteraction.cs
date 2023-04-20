using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class CustomInteraction : MonoBehaviour
{
    [SerializeField] private InputActionReference _Inputs;

    private Cauldron _cauldron;
    private Collider _foundCollider;

    private void OnEnable()
    {
        _Inputs.action.started += Select;
        _Inputs.action.canceled+= Release;
    }

    private void Select(InputAction.CallbackContext context)
    {
        if (_foundCollider != null && context.started)
        {
            Debug.Log("PICKUP");
            _cauldron = _foundCollider.gameObject.GetComponentInParent<Cauldron>();
            _cauldron.Interaction = this;
        }
    }

    private void Release(InputAction.CallbackContext context)
    {
        if (_foundCollider != null && context.canceled)
        {
            Debug.Log("DROP2");
            _foundCollider = null;
            if (_cauldron != null)
                _cauldron.Interaction = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Stick"))
        {
            _foundCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Stick"))
        {
            Debug.Log("RELEASE");
            if (_cauldron != null)
            {
                _cauldron.Interaction = null;
            }
            _foundCollider = null;
        }
    }

    public void ReleaseStick()
    {
        _foundCollider = null;
        _cauldron.Interaction = null;
    }
}
