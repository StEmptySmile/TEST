using Core.Object.Characteristic;
using UnityEditor;
using UnityEngine;

namespace CoreEditor.Object.Characteristic
{
    
    [CustomPropertyDrawer(typeof(CharacteristicComponent))]
    public sealed class CharacteristicDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + (EditorGUIUtility.singleLineHeight * 4 + 6) * (property.isExpanded ? 1 : 0);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                var labelRect = new Rect(position.x, position.y, position.width, 16);
                property.isExpanded = EditorGUI.Foldout(labelRect, property.isExpanded, label);
                EditorGUI.indentLevel++;
                {
                    if (property.isExpanded)
                    {
                        var valueTypeRect = new Rect(position.x, position.y + 20, position.width, 18);
                        var minValueRect = new Rect(position.x, position.y + 40, position.width, 18);
                        var maxValueRect = new Rect(position.x, position.y + 60, position.width, 18);
                        var sliderRect = new Rect(position.x, position.y + 80, position.width, 18);

                        EditorGUI.PropertyField(valueTypeRect, property.FindPropertyRelative("_valueType"));

                        SerializedProperty minValueProperty = property.FindPropertyRelative("_minValue");
                        EditorGUI.PropertyField(minValueRect, minValueProperty);
                        SerializedProperty maxValueProperty = property.FindPropertyRelative("_maxValue");
                        EditorGUI.PropertyField(maxValueRect, maxValueProperty);

                        if (minValueProperty.floatValue > maxValueProperty.floatValue)
                            minValueProperty.floatValue = maxValueProperty.floatValue;

                        float minValue = property.FindPropertyRelative("_minValue").floatValue;
                        float maxValue = property.FindPropertyRelative("_maxValue").floatValue;
                        property.FindPropertyRelative("_defaultValue").floatValue = Mathf.Clamp(property.FindPropertyRelative("_defaultValue").floatValue, minValue, maxValue);

                        EditorGUI.Slider(sliderRect, property.FindPropertyRelative("_defaultValue"), minValue, maxValue);

                        property.FindPropertyRelative("_currentValue").floatValue = property.FindPropertyRelative("_defaultValue").floatValue;
                    }
                }
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }
}