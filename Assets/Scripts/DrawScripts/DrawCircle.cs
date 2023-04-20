using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(Cauldron))]
public class DrawCircle : Editor
{
    private void OnSceneGUI()
    {
        Cauldron myCauldron = (Cauldron)target;
        Transform surface = myCauldron._WaterSurface;
        if (surface != null)
        {
            Handles.color = Color.red;
            Handles.DrawWireDisc(surface.position, myCauldron.transform.up, myCauldron._Radius);
            //Vector3 startPoint = transform.TransformPoint(_WaterSurface.position);
            //Vector3 baseVector = new Vector3(0, 0, 1);
            //startPoint += baseVector * _Radius;
            //float angle = 360.0f / (float)_NrOfEdges;
            //for (int i = 0; i <= _NrOfEdges; i++)
            //{
            //    float x = Mathf.Cos(Mathf.Deg2Rad * angle) * _Radius;
            //    float y = Mathf.Sin(Mathf.Deg2Rad * angle) * _Radius;
            //    Vector3 endPoint = _WaterSurface.position + (baseVector + new Vector3(x, 0, y));
            //    Gizmos.color = Color.red;
            //    Gizmos.DrawLine(startPoint, endPoint);
            //    startPoint = endPoint;
            //    if (i < _NrOfEdges)
            //    {
            //        angle += 360.0f / (float)_NrOfEdges;
            //    }
            //}
        }
    }
}
#endif
