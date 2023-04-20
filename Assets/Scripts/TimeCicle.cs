using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimeCicle : MonoBehaviour
{
    [SerializeField] private float _TimeSpeed = 1;
    public float TimeSpeed { get { return _TimeSpeed; } }
    [SerializeField] private float _DefaultCicleInMinutes = 20;

    private float _defaultTimeInMinutes = 1440;
    private float _defaultSpeedUpInSec = 1.0f;
    //start time 21600 = 6 hours
    private float _currentTime = 21600;
    private float _secondsInDay = 86400;
    private float _angularSpeed = 0;

    private UnityEvent _newDay = new UnityEvent();

    private Vector3 _currentRotation;
    private void Start()
    {
        _defaultSpeedUpInSec = _defaultTimeInMinutes / _DefaultCicleInMinutes;
        _angularSpeed = 360f / _secondsInDay;
        _currentRotation = transform.eulerAngles;
        _currentRotation.x = 0;
        transform.eulerAngles = _currentRotation;
    }
    private void Update()
    {
        _currentTime += Time.deltaTime * _defaultSpeedUpInSec * _TimeSpeed;
        _currentRotation.x += _angularSpeed * Time.deltaTime * _defaultSpeedUpInSec * _TimeSpeed;
        if (_currentRotation.x > 360)
            _currentRotation.x = _currentRotation.x - 360;
        transform.eulerAngles = _currentRotation;

        if (_currentTime > _secondsInDay )
        {
            _currentTime = _secondsInDay - _currentTime;
            _newDay.Invoke();
        }
    }

    public void AddListenerToNewDay(UnityAction action)
    {
        _newDay.AddListener(action);
    }

    public float GetCurrentHour()
    {
        float currentHour = _currentTime / 3600;
        return currentHour;
    }
}
