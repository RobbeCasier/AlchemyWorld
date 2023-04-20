using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : MonoBehaviour
{
    [Header("Default Parameters")]
    [SerializeField] public Transform _WaterSurface;
    [SerializeField] private Transform _StickTop;
    [SerializeField] private ParticleSystem _Ripple;

    private Vector3 _pointOnCircle;
    private Vector3 _oldPointOnCircle;

    private void Awake()
    {
        FindPointOnCircle(ref _pointOnCircle, transform.up);
        _oldPointOnCircle = _pointOnCircle;
    }
    private void FixedUpdate()
    {
        FindPointOnCircle(ref _pointOnCircle, transform.up);
        _Ripple.transform.position = _pointOnCircle;
    }

    public void UpdateRipple()
    {
        if (_pointOnCircle != _oldPointOnCircle)
        {
            Vector3 forward = _pointOnCircle - _oldPointOnCircle;
            forward.Normalize();
            //Quaternion lookAt = Quaternion.LookRotation(_pointOnCircle - _oldPointOnCircle);
            //int y = (int)lookAt.eulerAngles.y;
            //CreateRipple(lookAt.eulerAngles, y - 90, y + 90, 3, 0.1f, 0.1f, 5.0f);
        }
        _oldPointOnCircle = _pointOnCircle;
    }
    private void FindPointOnCircle(ref Vector3 pointInCircle, Vector3 up)
    {
        Vector3 vectorToCenter = _WaterSurface.position - transform.parent.position;
        //find the hypothenusa
        //get angle
        float angle = Vector3.Angle(Vector3.up, up);
        //get opposite
        float side = vectorToCenter.magnitude;
        float opposite = side * Mathf.Tan(Mathf.Deg2Rad * angle);

        //get hypothenuza
        float hypothenusa = Mathf.Sqrt(Mathf.Pow(opposite, 2) + Mathf.Pow(side, 2));

        //get 3d point of point in circle
        pointInCircle = transform.up * hypothenusa + transform.parent.position;
        //A = center of circle
        //B = point
        //r = radius
        //x = Ax + r * ((Bx - Ax)/sqrt((Bx - Ax)^2 + (By - Ay)^2))
        //float x = _WaterSurface.position.x + 0.265f * ((pointInCircle.x - _WaterSurface.position.x) / Mathf.Sqrt(Mathf.Pow(pointInCircle.x - _WaterSurface.position.x, 2) + Mathf.Pow(pointInCircle.z - _WaterSurface.position.z, 2)));
        //y = Ay + r * ((By - Ay)/sqrt((Bx - Ax)^2 + (By - Ay)^2))
        //float y = _WaterSurface.position.z + 0.265f * ((pointInCircle.z - _WaterSurface.position.z) / Mathf.Sqrt(Mathf.Pow(pointInCircle.x - _WaterSurface.position.x, 2) + Mathf.Pow(pointInCircle.z - _WaterSurface.position.z, 2)));

        //pointInCircle.x = x;
        //pointInCircle.z = y;
        //pointInCircle.y = _WaterSurface.position.y;
    }

    private void CreateRipple(Vector3 forwardEuler, int start, int end, int delta, float speed, float size, float lifeTime)
    {
        Vector3 forward = forwardEuler;
        forward.y = start;
        _Ripple.transform.eulerAngles = forward;
        for (int i = start; i < end; i += delta)
        {
            _Ripple.Emit(_pointOnCircle + _Ripple.transform.forward * 0.01f, _Ripple.transform.forward * speed, size, lifeTime, Color.white);
            _Ripple.transform.eulerAngles += _Ripple.transform.up * 3;
        }
    }
}
