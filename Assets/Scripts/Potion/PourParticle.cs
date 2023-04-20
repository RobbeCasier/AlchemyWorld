using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PourParticle : MonoBehaviour
{
    public int Amount { get; set; } //out of 100
    public PotionItem Potion { get; set;}
    [SerializeField] private MeshRenderer _MeshRenderer;

    private float _waterDensity = 0.00005f;
    private float _yVelocity = 0;

    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }
    public void FixedUpdate()
    {
        //_yVelocity = _yVelocity - (9.81f * Time.fixedDeltaTime * _waterDensity);
        //var pos = transform.position;
        //pos.y += _yVelocity;
        //transform.position = pos;
    }

    public void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position);
    }

    public void SetColor(Color color)
    {
        var material = _MeshRenderer.material;
        Material newMaterial = new Material(material);
        newMaterial.color = color;
        _MeshRenderer.material = newMaterial;
    }
}
