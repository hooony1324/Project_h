using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomPropertyDrawer(typeof(ButtonAttribute))]
public class ButtonDrawer: PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        string methodName = (attribute as ButtonAttribute).MethodName;
        Object target = property.serializedObject.targetObject;
        System.Type type = target.GetType();
        System.Reflection.MethodInfo method = type.GetMethod(methodName);

        ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;
        Color prevColor = GUI.backgroundColor;
        GUI.backgroundColor = buttonAttribute.GetColor();

        if (method == null)
        {
            GUI.Label(position, "Method could not be found. Is it public?");
            return;
        }
        if (method.GetParameters().Length > 0)
        {
            GUI.Label(position, "Method cannot have parameters.");
            return;
        }

        position.height = buttonAttribute.Height;

        if (GUI.Button(position, method.Name))
        {
            method.Invoke(target, null);
        }

        GUI.backgroundColor = prevColor;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ButtonAttribute buttonAttribute = (ButtonAttribute)attribute;
        return buttonAttribute.Height;
    }
}
#endif