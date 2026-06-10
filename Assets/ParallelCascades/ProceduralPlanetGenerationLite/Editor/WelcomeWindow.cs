using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace ParallelCascades.ProceduralPlanetGenerationLite.Editor
{
    public class WelcomeWindow : EditorWindow
    {
        [SerializeField] private VisualTreeAsset m_UXMLDocument;
        
        [MenuItem("Window/Parallel Cascades/Procedural Planet Generation - Lite Samples Pack Welcome Window", priority = 0)]
        public static void ShowWindow()
        {
            var window = GetWindow(typeof(WelcomeWindow), true, "Procedural Planet Generation - Lite Samples Pack", true);
            window.minSize = new Vector2(550, 350);
            window.maxSize = new Vector2(550, 350);
            window.Show();
        }

        private void CreateGUI()
        {
            m_UXMLDocument.CloneTree(rootVisualElement);
        }
    }
}