/*
Utility Class to create "read only" attributes for UnityEditorVariables.
Good to control which attributes are assignable and which are not.

Initial author: Christian Kessner <SpiegelEiXXL>
Initial creation date: 20th AUG 2018
Initial name: ReadOnlyAttribute.cs
Written for: Unity Free 2 Play RTS project
*/
using UnityEngine;
using System;

[AttributeUsage(AttributeTargets.Field)]
public class ReadOnlyAttribute : PropertyAttribute { }

#if UNITY_EDITOR
[UnityEditor.CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyAttributeDrawer : UnityEditor.PropertyDrawer
{
    public override float GetPropertyHeight(UnityEditor.SerializedProperty property, GUIContent label){
         return UnityEditor.EditorGUI.GetPropertyHeight (property, label, true) ;
    }
    public override void OnGUI(Rect rect, UnityEditor.SerializedProperty prop, GUIContent label)
    {
        bool wasEnabled = GUI.enabled;
        GUI.enabled = false;
        UnityEditor.EditorGUI.PropertyField(rect, prop, true);
        GUI.enabled = wasEnabled;
    }
}
#endif