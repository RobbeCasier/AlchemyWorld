using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Effect))]
[CanEditMultipleObjects]
public class EffectEditor : PropertyDrawer
{
    private SerializedProperty _effectsType;
    private SerializedProperty _healthType;
    private SerializedProperty _manaType;
    private SerializedProperty _buffType;
    private SerializedProperty _debuffType;

    private int propertyLines = 1;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);

        //fill properties
        _effectsType = property.FindPropertyRelative("Type");
        _healthType = property.FindPropertyRelative("HealthType");
        _manaType = property.FindPropertyRelative("ManaType");
        _buffType = property.FindPropertyRelative("BuffType");
        _debuffType = property.FindPropertyRelative("DebuffType");

        Rect foldOutBox = new Rect(position.min.x, position.min.y, position.size.x, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldOutBox, property.isExpanded, label);

        EditorGUI.indentLevel++;
        if (property.isExpanded)
        {
            DrawTypeProperty(position, _effectsType, new GUIContent("Type"));
            propertyLines++;
            switch ((EffectType)_effectsType.enumValueIndex)
            {
                case EffectType.HEALTH:
                    DrawTypeProperty(position, _healthType, new GUIContent("Health Type"));
                    break;
                case EffectType.MANA:
                    DrawTypeProperty(position, _manaType, new GUIContent("Mana Type"));
                    break;
                case EffectType.BUFF:
                    DrawTypeProperty(position, _buffType, new GUIContent("Buff Type"));
                    break;
                case EffectType.DEBUFF:
                    DrawTypeProperty(position, _debuffType, new GUIContent("Debuff Type"));
                    break;
                default:
                    break;
            }
        }
        propertyLines = 1;
        EditorGUI.indentLevel--;
        EditorGUI.EndProperty();
    }

    private void DrawTypeProperty(Rect position, SerializedProperty property, GUIContent label)
    {
        var rect = EditorGUI.IndentedRect(position);
        rect.y += propertyLines * EditorGUIUtility.singleLineHeight;

        EditorGUI.PropertyField(rect, property, label);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int totalLines = 1;

        if (property.isExpanded)
        {
            totalLines += 2;
        }
        
        return (EditorGUIUtility.singleLineHeight * totalLines);
    }
}
