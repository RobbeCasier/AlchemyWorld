using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSDrawer : MonoBehaviour
{
    [SerializeField] private TMP_Text _Text;

    private int _frameCounter = 0;
    private float _timeCounter = 0;
    private float _refreshTime = 1.0f;
    private void Update()
    {
        if (_timeCounter < _refreshTime)
        {
            _timeCounter += Time.deltaTime;
            _frameCounter++;
        }
        else
        {
            _Text.text = _frameCounter.ToString() + " FPS";
            _frameCounter = 0;
            _timeCounter= 0;
        }
    }
}
