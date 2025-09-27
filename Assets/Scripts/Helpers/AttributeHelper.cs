using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

/// <summary>
///  Usage:
///[ShowIf("myBoolField")] //it Shows the field only if myBoolField is true 
/// [ShowIf("myBoolField", true)] //it Shows the field only if myBoolField is false (inverted)
/// 
/// </summary>
public class ShowIfAttribute : PropertyAttribute
{
    public string ConditionField;
    public bool Invert;

    public ShowIfAttribute(string conditionField, bool invert = false)
    {
        ConditionField = conditionField;
        Invert = invert;
    }
}

[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        if (ShouldShow(property, showIf))
            EditorGUI.PropertyField(position, property, label, true);
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        return ShouldShow(property, showIf)
            ? EditorGUI.GetPropertyHeight(property, label, true)
            : 0f; // nascondo il campo
    }

    private bool ShouldShow(SerializedProperty property, ShowIfAttribute showIf)
    {
        // Trovo la proprietà booleana che fa da condizione
        SerializedProperty condition = property.serializedObject.FindProperty(
            property.propertyPath.Replace(property.name, showIf.ConditionField));

        if (condition == null)
        {
            Debug.LogWarning($"ShowIf: '{showIf.ConditionField}' non trovato per {property.name}");
            return true; // fallback: mostralo sempre
        }

        if (condition.propertyType == SerializedPropertyType.Boolean)
        {
            bool value = condition.boolValue;
            return showIf.Invert ? !value : value;
        }

        Debug.LogWarning($"ShowIf: '{showIf.ConditionField}' non è un bool!");
        return true;
    }
}

/// <summary>
/// Usage: 
/// [ShowIfEnum("myEnumField", "Value1", "Value2")] // Shows the field only if myEnumField is Value1 or Value2
/// 
/// 
/// </summary>
public class ShowIfEnumAttribute : PropertyAttribute
{
    public readonly string ConditionField;
    public readonly string[] AnyOfNames;
    public bool Invert { get; set; }

    public ShowIfEnumAttribute(string conditionField, params string[] anyOfNames)
    {
        ConditionField = conditionField;
        AnyOfNames = anyOfNames ?? new string[0];
        Invert = false;
    }
}

[CustomPropertyDrawer(typeof(ShowIfEnumAttribute))]
public class ShowIfEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var attr = (ShowIfEnumAttribute)attribute;
        bool show = ShouldShow(property, attr);

        if (show)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var attr = (ShowIfEnumAttribute)attribute;
        bool show = ShouldShow(property, attr);
        return show
            ? EditorGUI.GetPropertyHeight(property, label, true)
            : 0f; // nascondi
    }

    private bool ShouldShow(SerializedProperty property, ShowIfEnumAttribute attr)
    {
        // Trova il "sibling" in base al path (funziona anche in array/list)
        SerializedProperty cond = FindRelativeSibling(property, attr.ConditionField);
        if (cond == null)
        {
            Debug.LogWarning($"[ShowIfEnum] Campo '{attr.ConditionField}' non trovato accanto a '{property.propertyPath}'.");
            return true; // fallback: non nascondere nulla
        }

        if (cond.propertyType != SerializedPropertyType.Enum)
        {
            Debug.LogWarning($"[ShowIfEnum] '{attr.ConditionField}' non è un Enum (tipo: {cond.propertyType}).");
            return true;
        }

        string current = GetEnumName(cond);

        bool inList = (attr.AnyOfNames == null || attr.AnyOfNames.Length == 0)
            ? true // se non passi valori, consideriamo la condizione vera
            : attr.AnyOfNames.Any(n => string.Equals(n, current, StringComparison.OrdinalIgnoreCase));

        return attr.Invert ? !inList : inList;
    }

    private static string GetEnumName(SerializedProperty enumProp)
    {
        // Usa i nomi visualizzati corretti per la versione in uso
#if UNITY_2021_2_OR_NEWER
        var names = enumProp.enumDisplayNames;
#else
        var names = enumProp.enumNames;
#endif
        int idx = Mathf.Clamp(enumProp.enumValueIndex, 0, names.Length - 1);
        var result = (names != null && names.Length > 0) ? names[idx] : string.Empty;
        return result.Replace(" ", "");
    }

    /// <summary>
    /// Sostituisce l'ultimo segmento del propertyPath con "siblingName".
    /// Esempio:  "data.Array.data[0].campo" -> "data.Array.data[0].siblingName"
    /// </summary>
    private static SerializedProperty FindRelativeSibling(SerializedProperty property, string siblingName)
    {
        string path = property.propertyPath;
        if (string.IsNullOrEmpty(path)) return null;

        int lastDot = path.LastIndexOf('.');
        string newPath = lastDot >= 0 ? path.Substring(0, lastDot + 1) + siblingName : siblingName;
        return property.serializedObject.FindProperty(newPath);
    }
}


