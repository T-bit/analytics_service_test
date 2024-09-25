using System;
using TestEventService.Attributes;
using TestEventService.Extensions;
using UnityEditor;
using UnityEngine;

namespace TestEventService.Drawers
{
    [CustomPropertyDrawer(typeof(PolymorphicAttribute))]
    public sealed class PolymorphicPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return property.isExpanded
                ? EditorGUI.GetPropertyHeight(property,true)
                : EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var fieldType = property.GetFieldType();
            var valueType = property.GetValueType();
            var dropDownPosition = position;

            dropDownPosition.x += EditorGUIUtility.labelWidth + 2;
            dropDownPosition.width -= EditorGUIUtility.labelWidth + 2;
            dropDownPosition.height = EditorGUIUtility.singleLineHeight;

            if (EditorGUI.DropdownButton(dropDownPosition, new GUIContent(valueType?.Name ?? "NOT SET"), FocusType.Keyboard))
            {
                var menu = new GenericMenu();

                foreach (var type in fieldType.GetDerivedTypes())
                {
                    if (type == null)
                    {
                        continue;
                    }

                    var sameValue = type == valueType;

                    menu.AddItem(new GUIContent(type.Name), sameValue, OnValueChanged);

                    continue;

                    void OnValueChanged()
                    {
                        if (sameValue)
                        {
                            return;
                        }

                        property.managedReferenceValue = type.GetConstructor(Type.EmptyTypes)?.Invoke(null);
                        property.serializedObject.ApplyModifiedProperties();
                    }
                }

                menu.ShowAsContext();
            }

            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, position.height), property, label, true);
        }
    }
}