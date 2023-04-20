using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(RemoteEventTrigger))]
[CanEditMultipleObjects]
public class RemoteEventTriggerEditor : Editor
{
    private SerializedProperty _DefaultEvent;
    private SerializedProperty _OnTriggerEnter;
    private SerializedProperty _OnTriggerExit;
    
    private SerializedProperty _ColliderEvent;
    private SerializedProperty _OnTriggerEnterCollider;
    private SerializedProperty _OnTriggerExitCollider;

    private SerializedProperty _IntEvent;
    private SerializedProperty _OnTriggerEnterInt;
    private SerializedProperty _OnTriggerExitInt;

    //Extra Options
    private SerializedProperty _LayerCheck;
    private SerializedProperty _Layers;

    private SerializedProperty _TagCheck;
    private SerializedProperty _TagName;

    void OnEnable()
    {
        _DefaultEvent = serializedObject.FindProperty("DefaultEvent");
        _OnTriggerEnter = serializedObject.FindProperty("_OnTriggerEnter");
        _OnTriggerExit = serializedObject.FindProperty("_OnTriggerExit");

        _ColliderEvent = serializedObject.FindProperty("ColliderEvent");
        _OnTriggerEnterCollider = serializedObject.FindProperty("_OnTriggerEnterCollider");
        _OnTriggerExitCollider = serializedObject.FindProperty("_OnTriggerExitCollider");

        _IntEvent = serializedObject.FindProperty("ParameterEvent");
        _OnTriggerEnterInt = serializedObject.FindProperty("_OnTriggerEnterParameter");
        _OnTriggerExitInt = serializedObject.FindProperty("_OnTriggerExitParameter");

        //Extra Options
        _LayerCheck = serializedObject.FindProperty("LayerCheck");
        _Layers = serializedObject.FindProperty("Layers");

        _TagCheck = serializedObject.FindProperty("TagCheck");
        _TagName = serializedObject.FindProperty("TagName");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_DefaultEvent);
        if (_DefaultEvent.boolValue)
        {
            EditorGUILayout.PropertyField(_OnTriggerEnter);
            EditorGUILayout.PropertyField(_OnTriggerExit);
        }

        EditorGUILayout.PropertyField(_ColliderEvent, new GUIContent("Collider Event"));
        if (_ColliderEvent.boolValue)
        {
            EditorGUILayout.PropertyField(_OnTriggerEnterCollider);
            EditorGUILayout.PropertyField(_OnTriggerExitCollider);
        }
        
        EditorGUILayout.PropertyField(_IntEvent, new GUIContent("Parameter Event"));
        if (_IntEvent.boolValue)
        {
            EditorGUILayout.PropertyField(_OnTriggerEnterInt);
            EditorGUILayout.PropertyField(_OnTriggerExitInt);
        }

        //Extra Options
        EditorGUILayout.PropertyField(_LayerCheck, new GUIContent("Layer Check"));
        if (_LayerCheck.boolValue)
        {
            EditorGUILayout.PropertyField(_Layers);
        }

        EditorGUILayout.PropertyField(_TagCheck, new GUIContent("Tag Check"));
        if (_TagCheck.boolValue)
        {
            EditorGUILayout.PropertyField(_TagName);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
