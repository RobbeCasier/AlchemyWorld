using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class TeleportationManager : MonoBehaviour
{
    [SerializeField] private InputActionAsset actionAsset;
    [SerializeField] private XRRayInteractor _RayInteractor;
    [SerializeField] private TeleportationProvider _TeleportationProvider;

    private InputAction _thumbstick;
    private bool _isActive;

    private void Start()
    {
        _RayInteractor.enabled = false;

        var activate = actionAsset.FindActionMap("XRI LeftHand Locomotion").FindAction("Teleport Mode Activate");
        activate.Enable();
        activate.performed += OnTeleportActivate;

        var cancel = actionAsset.FindActionMap("XRI Lefthand Locomotion").FindAction("Teleport Mode Cancel");
        cancel.Enable();
        cancel.performed += OnTeleportCancel;

        _thumbstick = actionAsset.FindActionMap("XRI Lefthand Locomotion").FindAction("Move");
        _thumbstick.Enable();
    }

    [System.Obsolete]
    private void Update()
    {
        if (!_isActive)
            return;

        if (_thumbstick.triggered)
            return;

        if (!_RayInteractor.GetCurrentRaycastHit(out RaycastHit hit))
        {
            _RayInteractor.enabled = false;
            _isActive = false;
        }

        TeleportRequest request = new TeleportRequest()
        {
            destinationPosition = hit.point
        };

        _TeleportationProvider.QueueTeleportRequest(request);
    }

    private void OnTeleportActivate(InputAction.CallbackContext context)
    {
        _RayInteractor.enabled = true;
        _isActive = true;
    }

    private void OnTeleportCancel(InputAction.CallbackContext context)
    {
        _RayInteractor.enabled = false;
        _isActive = false;
    }
}
