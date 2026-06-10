using System.IO;
using UnityEditor;

namespace ParallelCascades.ProceduralShaders.Editor
{
    public static class CreateGradientTextureMenuItem
    {
        [MenuItem("Assets/Create/Parallel Cascades/Color Gradient Texture", false, 10)]
        private static void CreateGradientTexture()
        {
            string filePath;

            if (Selection.assetGUIDs.Length == 0)
                filePath = "Assets/";
            else
                filePath = AssetDatabase.GUIDToAssetPath(Selection.assetGUIDs[0]);

            if (!Directory.Exists(filePath))
            {
                filePath = Path.GetDirectoryName(filePath);
            }
            
            var colorGradient = TextureUtilities.CreateGradientTexture(filePath, "Color Gradient Texture");
            
            EditorUtility.FocusProjectWindow();
            EditorGUIUtility.PingObject(colorGradient);        
        }
    }
}