using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(InteractionComponent))]
public class InteractionComponentDrawer : PropertyDrawer {

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return 32;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        position.height = 16;

        EditorGUIUtility.labelWidth = 50f;
        SerializedObject SO = new SerializedObject(property.objectReferenceValue);

        //SerializedProperty myProp = SO.FindProperty("asd");
        //myProp.vector3Value = EditorGUI.Vector3Field(position, "asd: ", myProp.vector3Value);

        //myProp = SO.FindProperty("qwe");
        //position.y += 16;
        //myProp.floatValue = EditorGUI.FloatField(position, "qwe: ", myProp.floatValue);

        //position.y += 16;
        //myProp.arraySize = EditorGUI.IntField(position, "Size: ", myProp.arraySize);

        SerializedProperty myProp = SO.FindProperty("messages");
        EditorGUI.PropertyField(position, myProp, true);

        SO.ApplyModifiedProperties();
    }
}
