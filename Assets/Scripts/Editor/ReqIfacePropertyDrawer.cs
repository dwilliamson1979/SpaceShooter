using UnityEditor;
using UnityEngine;
using com.dhcc.utility;
using System.Linq;

[CustomPropertyDrawer(typeof(ReqIfaceAttribute))]
public class ReqIfacePropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginChangeCheck();
        EditorGUI.PropertyField(position, property, label);
        if (EditorGUI.EndChangeCheck())
        {
            ReqIfaceAttribute attr = attribute as ReqIfaceAttribute;
            GameObject go = property.objectReferenceValue as GameObject;
            if (go != null && attr != null)
            {
                if (go.GetComponent(attr.InterfaceType) == null)
                {
                    Debug.LogWarning($"{property.serializedObject.targetObject.name}.{property.name} does not implement the {attr.InterfaceType.Name} interface!");
                    property.objectReferenceValue = null;
                }
            }
        }
    }
}