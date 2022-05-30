using Core.Object.Status;
using UnityEditor;
using UnityEngine;

namespace CoreEditor.Object.Status
{
    [CustomPropertyDrawer(typeof(StatusComponent))]
    public sealed class StatusDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.singleLineHeight * (property.isExpanded ? 1 : 0);
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
                        var maxValueRect = new Rect(position.x, position.y + 20, position.width, 18);

                        EditorGUI.PropertyField(maxValueRect, property.FindPropertyRelative("_maxValue"));
                        float maxValue = property.FindPropertyRelative("_maxValue").floatValue;
                        property.FindPropertyRelative("_currentMaxValue").floatValue = maxValue;
                        property.FindPropertyRelative("_currentValue").floatValue = maxValue;
                    }
                }      
                EditorGUI.indentLevel--;
            }
            EditorGUI.EndProperty();
        }
    }
}
