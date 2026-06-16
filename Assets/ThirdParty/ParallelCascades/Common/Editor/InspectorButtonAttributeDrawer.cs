using System.Reflection;
using ParallelCascades.Common.Runtime;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ParallelCascades.Common.Editor
{
    [UnityEditor.CustomPropertyDrawer(typeof(InspectorButtonAttribute))]
    public class InspectorButtonAttributeDrawer : UnityEditor.PropertyDrawer
    {
        public override UnityEngine.UIElements.VisualElement CreatePropertyGUI(UnityEditor.SerializedProperty property)
        {
            var root = new VisualElement();
            
            InspectorButtonAttribute attr = (InspectorButtonAttribute)attribute;
            
            MethodInfo boundMethod = property.serializedObject.targetObject.GetType()
                .GetMethod(attr.methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (boundMethod != null)
            {
                root.Add(new Button(() =>
                {
                    UnityEditor.Undo.RecordObject(property.serializedObject.targetObject, $"Invoke {boundMethod.Name}");
                    boundMethod?.Invoke(property.serializedObject.targetObject, null);
                    UnityEditor.EditorUtility.SetDirty(property.serializedObject.targetObject);
                })
                {
                    text = attr.buttonText
                });
            }
            else
            {
                root.Add(new Label($"No method with name {attr.methodName}"));
            }
            
            root.Add(new PropertyField(property));
            
            return root;
        }
    }
}