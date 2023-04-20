using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Potion : MonoBehaviour
{
    [SerializeField] private XRGrabInteractable _GrabInteractible;
    [SerializeField] private XRSocketInteractor _SocketInteractible;

    [SerializeField] private int _PourStartAngle = 30;

    [SerializeField] private LiquidStream _LiquidStream;
    [SerializeField] private int _Capacity = 200;

    [Header("Sound")]
    [SerializeField] private EventReference _CorkOn;
    [SerializeField] private EventReference _CorkOff;

    private bool _isPouring = false;
    private bool _isCorked = false;

    public int _capacityInMl { get; private set; }

    public GameObject _cork { private get; set; }
    public PotionItem _item { get; private set; } = new PotionItem();

    private void Start()
    {
        _capacityInMl = _Capacity;
        _item.SetCapacity(_Capacity);
        _SocketInteractible.interactionManager = _GrabInteractible.interactionManager = ItemManager._instance.InteractionManager;

        _LiquidStream.SetMinimumAngle(_PourStartAngle);
    }
    
    public void RemoveSelf()
    {
        Destroy(_cork.gameObject);
        Destroy(this.gameObject);
    }
    private void FixedUpdate()
    {
        if (_isCorked)
            return;
        var pourAngle = CalculatePourAngle();
        bool pourCheck = pourAngle < _PourStartAngle;

        if (!_isPouring != pourCheck)
        {
            _isPouring = !_isPouring;
        }
        if (_isPouring) StartPour(); else EndPour();
    }

    private void StartPour()
    {
        _LiquidStream.Pour(CalculatePourAngle());

    }

    private void EndPour()
    {
        _LiquidStream.StopPour();
    }

    private float CalculatePourAngle()
    {
        return Vector3.Angle(transform.up, Vector3.up);
    }

    public void DisableCork()
    {
        if (_cork != null)
            _cork.GetComponentInChildren<MeshRenderer>().enabled = false;
        _isCorked = true;

        RuntimeManager.PlayOneShot(_CorkOn, _SocketInteractible.transform.position);
    }

    public void EnableCork()
    {
        if (_cork != null)
            _cork.GetComponentInChildren<MeshRenderer>().enabled = true;
        _isCorked = false;
        RuntimeManager.PlayOneShot(_CorkOff, _SocketInteractible.transform.position);

        _cork = null;
    }

    public void InitializePotion(PotionItem potion)
    {
        _item = potion;
    }
}
