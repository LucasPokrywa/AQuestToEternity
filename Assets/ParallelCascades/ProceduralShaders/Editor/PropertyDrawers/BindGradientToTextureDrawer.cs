using ParallelCascades.ProceduralShaders.Runtime.PropertyAttributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ParallelCascades.ProceduralShaders.Editor.PropertyDrawers
{
    [CustomPropertyDrawer(typeof(BindGradientToTextureAttribute))]
    public class BindGradientToTextureDrawer : PropertyDrawer
    {
        private static string _invalidTypeMessage = "Use [BindGradientToTexture] with gradient.";

        private string _texturePropertyName;
        private string _parentPropertyName;
        private float _saveDelay;
        
        private float _lastChangeTime;
        
        private Gradient _gradient;
        private Texture2D _texture;
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            if(property.propertyType != SerializedPropertyType.Gradient)
                return new Label(_invalidTypeMessage);

            BindGradientToTextureAttribute gradientToTexAttr = (BindGradientToTextureAttribute)this.attribute;
            _texturePropertyName = gradientToTexAttr.TexturePropertyName;
            _parentPropertyName = gradientToTexAttr.ParentPropertyName;
            _saveDelay = gradientToTexAttr.SaveDelay;
            
            var gradientField = new GradientField(property.displayName)
            {
                hdr = gradientToTexAttr.HDR,
                colorSpace = gradientToTexAttr.ColorSpace,
                bindingPath = property.propertyPath
            };
            gradientField.AddToClassList(BaseField<Gradient>.alignedFieldUssClassName);

            gradientField.RegisterValueChangedCallback(evt =>
            {
                Undo.RecordObject(property.serializedObject.targetObject, "Update Gradient");
                _gradient = evt.newValue;
                CaptureTexture(property);
                if (_texture)
                {
                    RuntimeTextureUtilities.SetTextureFromGradient(_gradient, _texture);
                    ScheduleSave();
                }
            });
            
            container.Add(gradientField);

            return container;
        }

        // IMGUI fallback when UIToolkit is not available
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Gradient)
            {
                EditorGUI.LabelField(position, _invalidTypeMessage);
                return;
            }

            BindGradientToTextureAttribute gradientToTexAttr = (BindGradientToTextureAttribute)this.attribute;
            _texturePropertyName = gradientToTexAttr.TexturePropertyName;
            _saveDelay = gradientToTexAttr.SaveDelay;

            // This is necessary to actually update the texture
            CaptureTexture(property);

            EditorGUI.BeginChangeCheck();
            Gradient gradientValue = EditorGUI.GradientField(position, label, property.gradientValue, gradientToTexAttr.HDR);
            
            // Undo functionality
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(property.serializedObject.targetObject, "Update Gradient");
                if (_texture != null)
                {
                    Undo.RecordObject(_texture, "Update Texture from Gradient");
                }
                property.gradientValue = gradientValue;
                _gradient = gradientValue;
                if (_texture != null)
                {
                    RuntimeTextureUtilities.SetTextureFromGradient(_gradient, _texture);
                    EditorUtility.SetDirty(_texture);
                }
                ScheduleSave();
            }
        }

        private void CaptureTexture(SerializedProperty property)
        {
            SerializedProperty textureProperty;
            if (_parentPropertyName != null)
            {
                textureProperty = property.serializedObject.FindProperty(_parentPropertyName).FindPropertyRelative(_texturePropertyName);
            }
            else
            {
                textureProperty = property.serializedObject.FindProperty(_texturePropertyName);
            }
            if (textureProperty == null) return;
                    
            _texture = textureProperty.objectReferenceValue as Texture2D;
        }

        private void ScheduleSave()
        {
            _lastChangeTime = Time.realtimeSinceStartup;
            EditorApplication.update -= SaveTextureAsset;
            EditorApplication.update += SaveTextureAsset;
        }

        private void SaveTextureAsset()
        {
            if (Time.realtimeSinceStartup - _lastChangeTime > _saveDelay)
            {
                if (_texture == null) return;
                    
                Undo.RecordObject(_texture, "Update Texture from Gradient");
                RuntimeTextureUtilities.SetTextureFromGradient(_gradient, _texture);
                Undo.RecordObject(_texture, "Save Texture Asset");
                TextureUtilities.SaveTextureAsset(_texture);
                EditorApplication.update -= SaveTextureAsset;
            }
        }
    }
}