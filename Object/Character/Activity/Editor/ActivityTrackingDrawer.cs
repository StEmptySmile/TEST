using System.Collections.Generic;
using Core.Object.Character.Activity;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace CoreEditor.Object.Character.Activity
{
    [CustomPropertyDrawer(typeof(ActivityTrackingComponent))]
    public sealed class ActivityTrackingDrawer : PropertyDrawer
    {
        private int NumberValuesInTypesActivity => Enum.GetValues(typeof(TypesActivity)).Length;

        private int _changeableNumberLines = 0;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SerializedProperty animator = property.FindPropertyRelative("_animator");
            float height = EditorGUIUtility.singleLineHeight;
            if (animator.objectReferenceValue != null)
            {
                height += EditorGUIUtility.singleLineHeight;
                if (animator.isExpanded)
                {
                    height += _changeableNumberLines * (EditorGUIUtility.singleLineHeight + 2);
                }
            }
            return height;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            {
                Rect rect = new(position.x, position.y, position.width, 18);

                SerializedProperty animator = property.FindPropertyRelative("_animator");
                EditorGUI.PropertyField(rect, animator);

                if (animator.objectReferenceValue != null)
                {
                    CreateLine(ref rect);
                    animator.isExpanded = EditorGUI.Foldout(rect, animator.isExpanded, "Activities");

                    if (animator.isExpanded)
                    {
                        SerializedProperty activities = property.FindPropertyRelative("_activities");

                        if (activities.arraySize != NumberValuesInTypesActivity)
                            FillingWithDefaultData(activities);

                        _changeableNumberLines = 1;

                        DisplayListActivities(ref rect, animator.objectReferenceValue as Animator, activities);

                        CreateLine(ref rect);
                        if (GUI.Button(rect, "Add activity"))
                        {
                            AddNewActivity(activities);
                        }
                    }
                    else
                    {
                        _changeableNumberLines = 0;
                    }
                }
            }
            EditorGUI.EndProperty();
        }
        private void DisplayListActivities(ref Rect rect, Animator animator, SerializedProperty activities)
        {
            EditorGUI.indentLevel++;
            string[] namesAnimatorControllerParameter = new string[] { "None" };
            namesAnimatorControllerParameter = namesAnimatorControllerParameter.Concat(animator.parameters.Select(x => x.name)).ToArray();

            for (int index = 0; index < activities.arraySize; index++)
            {
                SerializedProperty property = activities.GetArrayElementAtIndex(index);
                SerializedProperty type = property.FindPropertyRelative("_type");

                if ((TypesActivity)type.enumValueIndex != TypesActivity.None)
                {
                    _changeableNumberLines += 1;

                    SerializedProperty indexAnimatorControllerParameter = property.FindPropertyRelative("_indexAnimatorControllerParameter");
                    TypesActivity typeActivity = (TypesActivity)type.enumValueIndex;

                    CreateLine(ref rect);
                    int number = EditorGUI.Popup(rect, typeActivity.ToString(), indexAnimatorControllerParameter.intValue + 1, namesAnimatorControllerParameter.ToArray());
                    indexAnimatorControllerParameter.intValue = number - 1;
                }
            }
            EditorGUI.indentLevel--;
        }
        private void FillingWithDefaultData(SerializedProperty activities)
        {
            int number = NumberValuesInTypesActivity - activities.arraySize;
            activities.arraySize += number;

            if (number > 0)
            {
                int defaultValueForType = GetEnumIndex(TypesActivity.None);
                for (int index = 0; index < activities.arraySize; index++)
                {
                    SerializedProperty property = activities.GetArrayElementAtIndex(index);
                    property.FindPropertyRelative("_type").enumValueIndex = defaultValueForType;
                }
            }
        }
        private void AddNewActivity(SerializedProperty activities)
        {
            List<TypesActivity> busyTypes = new();
            for (int index = 0; index < activities.arraySize; index++)
            {
                SerializedProperty property = activities.GetArrayElementAtIndex(index);
                SerializedProperty type = property.FindPropertyRelative("_type");
                TypesActivity typeActivity = (TypesActivity)type.enumValueIndex;
                busyTypes.Add(typeActivity);
            }
            IReadOnlyList<TypesActivity> freeTypes = GetFreeTypes(busyTypes);
            if (freeTypes.Count > 0)
            {
                int enumValueIndex = GetEnumIndex(freeTypes[0]);
                SerializedProperty property = activities.GetArrayElementAtIndex(enumValueIndex);
                SerializedProperty type = property.FindPropertyRelative("_type");
                type.enumValueIndex = enumValueIndex;
            }
        }
        private IReadOnlyList<TypesActivity> GetFreeTypes(IReadOnlyList<TypesActivity> busyTypes)
        {
            List<TypesActivity> answer
                = new((TypesActivity[])Enum.GetValues(typeof(TypesActivity)));

            return answer.Where(x => busyTypes.Contains(x) == false).ToList();
        }
        private int GetEnumIndex(TypesActivity value)
        {
            return Array.IndexOf(Enum.GetValues(typeof(TypesActivity)), value);
        }
        private void CreateLine(ref Rect rect)
        {
            rect = new Rect(rect.x, rect.y + 20, rect.width, 18);
        }
    }
}
