using UnityEditor;
using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Editor
{
    public static class EditorAssetUtility
    {
        public static bool ChooseProjectRelativeFolderPath(out string relativeFolderPath, string savePanelTitle = "Select Folder to Save Assets" )
        {
            string folderPath = EditorUtility.SaveFolderPanel(savePanelTitle, "Assets", "");
            if (string.IsNullOrEmpty(folderPath))
            {
                relativeFolderPath = string.Empty;
                return false;
            }

            if (!folderPath.StartsWith(Application.dataPath))
            {
                Debug.Log("Please select a folder inside the Assets folder.");
                relativeFolderPath = string.Empty;
                return false;
            }

            relativeFolderPath = "Assets" + folderPath.Substring(Application.dataPath.Length);
            return true;
        }

        public static void SaveAssetsAndFocusFolder(string projectRelativeFolderPath)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            
            Object folder = AssetDatabase.LoadAssetAtPath<Object>(projectRelativeFolderPath);
            Selection.activeObject = folder;
        }
        
        public static GameObject CreateGameObject(GameObject parent, string name, bool placeInFrontOfCamera = false)
        {
            GameObject go = new GameObject(name);
            GameObjectUtility.SetParentAndAlign(go, parent);
            GameObjectUtility.EnsureUniqueNameForSibling(go);
            
            if (placeInFrontOfCamera &&
                SceneView.lastActiveSceneView != null)
            {
                SceneView sceneView = SceneView.lastActiveSceneView;
                var transform = sceneView.camera.transform;
                go.transform.position = transform.position + transform.forward * 1.75f;
            }

            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            return go;
        }
        
        public static Material CreateMaterial(string projectRelativeFolderPath, string shaderName, string defaultMaterialName)
        {
            Material material = new Material(Shader.Find(shaderName));
            
            string materialPath = $"{projectRelativeFolderPath}/{defaultMaterialName}.mat";
            materialPath = AssetDatabase.GenerateUniqueAssetPath(materialPath);
            
            AssetDatabase.CreateAsset(material, materialPath);
            return material;
        }
    }
}