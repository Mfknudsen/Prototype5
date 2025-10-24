using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorInspector
{
#if UNITY_EDITOR
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class SerializeReferenceDrawer : PropertyAttribute
    {
    }

    [CustomPropertyDrawer(typeof(SerializeReferenceDrawer))]
    public class SerializeReferencePropertyDrawer : PropertyDrawer
    {
        private static readonly Dictionary<Type, Type[]> _derivedTypesCache = new Dictionary<Type, Type[]>();

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.ManagedReference)
            {
                EditorGUI.PropertyField(position, property, label);
                return;
            }

            EditorGUI.BeginProperty(position, label, property);

            Type baseType = GetFieldType(property);
            if (baseType == null)
            {
                EditorGUI.PropertyField(position, property, label);
                EditorGUI.EndProperty();
                return;
            }

            if (!_derivedTypesCache.TryGetValue(baseType, out var derivedTypes))
            {
                derivedTypes = GetDerivedTypes(baseType).ToArray();
                _derivedTypesCache[baseType] = derivedTypes;
            }

            Type currentType = property.managedReferenceValue?.GetType();
            string currentTypeName = currentType?.Name ?? "Null";

            List<string> typeNames = derivedTypes.Select(t => t.Name).ToList();
            typeNames.Insert(0, "Null");

            Rect popupRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            int currentIndex = typeNames.IndexOf(currentTypeName);
            int newIndex = EditorGUI.Popup(popupRect, label.text, currentIndex, typeNames.ToArray());

            if (newIndex != currentIndex)
            {
                if (newIndex == 0)
                {
                    property.managedReferenceValue = null;
                }
                else
                {
                    Type newType = derivedTypes[newIndex - 1];
                    property.managedReferenceValue = Activator.CreateInstance(newType);
                }

                property.serializedObject.ApplyModifiedProperties();
            }

            if (property.managedReferenceValue != null)
            {
                EditorGUI.indentLevel++;
                SerializedProperty iterator = property.Copy();
                SerializedProperty endProperty = iterator.GetEndProperty();
                bool enterChildren = true;

                while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, endProperty))
                {
                    EditorGUILayout.PropertyField(iterator, true);
                    enterChildren = false;
                }

                EditorGUI.indentLevel--;
            }

            EditorGUI.EndProperty();
        }

        private static Type GetFieldType(SerializedProperty property)
        {
            string[] paths = property.propertyPath.Split('.');
            Type type = property.serializedObject.targetObject.GetType();
            for (int i = 0; i < paths.Length; i++)
            {
                if (paths[i] == "Array")
                {
                    i++;
                    type = type.GetElementType() ?? type.GetGenericArguments()[0];
                }
                else
                {
                    FieldInfo field = type.GetField(paths[i],
                        BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (field == null) return null;
                    type = field.FieldType;
                }
            }

            return type;
        }

        private static IEnumerable<Type> GetDerivedTypes(Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(assembly => assembly.GetTypes())
                .Where(type => baseType.IsAssignableFrom(type) && !type.IsAbstract && type != baseType);
        }
    }
#endif
}