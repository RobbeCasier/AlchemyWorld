using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class TeleportController : MonoBehaviour
{
    [SerializeField] private GameObject _BaseControllerGameObject;
    [SerializeField] private GameObject _TeleportationGameObject;
    [SerializeField] private InputActionReference _TeleportActivationReference;

    [Space]
    public UnityEvent onTeleportActivate;
    public UnityEvent onTeleportDeactivate;

    private void Start()
    {
        _TeleportActivationReference.action.performed += TeleportModeActivate;
        _TeleportActivationReference.action.canceled += TeleportModeCancel;
    }

    private void TeleportModeCancel(InputAction.CallbackContext obj)
    {
        Invoke("DeactivateTeleporter", 0.1f);
    }

    private void DeactivateTeleporter()
    {
        onTeleportDeactivate.Invoke();
    }

    private void TeleportModeActivate(InputAction.CallbackContext obj)
    {
        onTeleportActivate.Invoke();
    }
}
