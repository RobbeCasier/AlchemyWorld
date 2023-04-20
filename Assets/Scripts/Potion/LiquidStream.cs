using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidStream : MonoBehaviour
{
    [SerializeField] private int _ParticleDivision = 20;
    [SerializeField] private float _MaxAngleSpawnTime = 0.01f;
    [SerializeField] private float _LowAngleSpawnTime = 1f;
    [SerializeField] private float _MinStartSpeed = 0.0f;
    [SerializeField] private float _MaxStartSpeed = 0.5f;
    [SerializeField] private float _MinStartSize= 0.01f;
    [SerializeField] private float _MaxStartSize = 0.05f;
    [SerializeField] private ValueDistributor _ValueDistributor;
    [SerializeField] private Potion _Potion;
    [SerializeField] private ParticleSystem _WaterParticleSystem;
    [SerializeField] private AnimationCurve _SpawnSpeedCurve;

    private float _currentSpawnTime = 0.0f;
    private float _currentElapsedTime = 0.0f;
    private uint _currentNrOfDistributions = 0;
    private bool _isPouring = false;

    private float _minimumAngle;

    private void Start()
    {
        _ValueDistributor.Value = _Potion._capacityInMl;
        _ValueDistributor.MaxDistribution = _ParticleDivision;
        ParticleSystemRenderer particleRenderer = _WaterParticleSystem.GetComponent<ParticleSystemRenderer>();
        Material mat = particleRenderer.material;
        mat.color = _Potion._item.LiquidColor;
        particleRenderer.material = particleRenderer.trailMaterial = mat;
    }

    // get the difference in order 
    public void SetMinimumAngle(float angle)
    {
        _minimumAngle = angle;
    }

    public void Pour(float angle)
    {
        if (_currentNrOfDistributions == _ParticleDivision)
            return;
        if (!_isPouring)
        {
            _WaterParticleSystem.Play();
        }
        _isPouring = true;
        float t = _SpawnSpeedCurve.Evaluate((angle - _minimumAngle) / (180f - _minimumAngle));
        _currentSpawnTime = Mathf.Lerp(_LowAngleSpawnTime, _MaxAngleSpawnTime, t);

        ParticleSystem.MainModule main = _WaterParticleSystem.main;
        main.startSpeed = Mathf.Lerp(_MinStartSpeed, _MaxStartSpeed, t);
        main.startSize = Mathf.Lerp(_MinStartSize, _MaxStartSize, t);

    }

    public void StopPour()
    {
        if (_isPouring)
        {
            _WaterParticleSystem.Stop();
        }
        _isPouring = false;
        _currentElapsedTime = 0;
    }

    private void Update()
    {
        if (!_isPouring)
            return;
        _currentElapsedTime += Time.deltaTime;
        while (_currentElapsedTime >= _currentSpawnTime)
        {
            _WaterParticleSystem.Emit((int)(_currentElapsedTime / _currentSpawnTime));
            //var particle = _PourParticleSystem.SpawnObject();
            //var pour = particle.GetComponent<PourParticle>();
            //pour.Potion = _Potion._item;
            //pour.Amount = _ValueDistributor.GetDistributedValue(_currentNrOfDistributions);
            _currentNrOfDistributions++;
            _currentElapsedTime -= ((float)(int)(_currentElapsedTime /_currentSpawnTime)) * _currentSpawnTime;
            if (_currentNrOfDistributions == _ParticleDivision)
            {
                _WaterParticleSystem.Stop();
                _isPouring = false;
            }
        }
    }
}
