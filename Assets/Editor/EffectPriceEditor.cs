using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

[CustomEditor(typeof(EffectPrice))]
[CanEditMultipleObjects]
public class EffectPriceEditor : Editor
{
    private SerializedProperty _EffectType;
    private SerializedProperty _HealthType;
    private SerializedProperty _ManaType;
    private SerializedProperty _BuffType;
    private SerializedProperty _DebuffType;
    private SerializedProperty _DefaultBuyPrice;
    private SerializedProperty _DefaultSellPrice;

    void OnEnable()
    {
        _EffectType = serializedObject.FindProperty("EffectType");

        _HealthType = serializedObject.FindProperty("HealthType");
        _ManaType = serializedObject.FindProperty("ManaType");
        _BuffType = serializedObject.FindProperty("BuffType");
        _DebuffType = serializedObject.FindProperty("DebuffType");

        _DefaultBuyPrice = serializedObject.FindProperty("DefaultBuyPrice");
        _DefaultSellPrice = serializedObject.FindProperty("DefaultSellPrice");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.PropertyField(_EffectType);
        switch (_EffectType.enumValueIndex)
        {
            case 0:
                EditorGUILayout.PropertyField(_HealthType);
                break;
            case 1:
                EditorGUILayout.PropertyField(_ManaType);
                break;
            case 2:
                EditorGUILayout.PropertyField(_BuffType);
                break;
            case 3:
                EditorGUILayout.PropertyField(_DebuffType);
                break;
            default:
                break;
        }

        EditorGUILayout.PropertyField(_DefaultBuyPrice, new GUIContent("Default Buy Price"));
        EditorGUILayout.PropertyField(_DefaultSellPrice, new GUIContent("Default Sell Price"));

        serializedObject.ApplyModifiedProperties();
    }
}
