using UnityEditor;
using UnityEngine;

namespace ParallelCascades.ProceduralShaders.Editor
{
    public static class CubemapUtilities
    {
        private static readonly int s_cubemapFace = Shader.PropertyToID("_Cubemap_Face");
        
        /// <summary>
        /// This requires a compatible material with a shader that has the _Cubemap_Face property to switch faces for rendering.
        /// </summary>
        public static Cubemap GetCubemapTextureFromMaterial(Material material, int faceSize)
        {
            Cubemap cubemap = new Cubemap(faceSize, TextureFormat.ARGB32, false);
            
            RenderTexture renderTexture = RenderTexture.GetTemporary(faceSize, faceSize, 16);
            RenderTexture.active = null;
            
            GameObject tempCameraObj = new GameObject
            {
                transform = { position = Vector3.back },
                hideFlags = HideFlags.HideAndDontSave
            };
            
            Camera tempCamera = tempCameraObj.AddComponent<Camera>();
            tempCamera.enabled = false;
            tempCamera.cameraType = CameraType.Preview;
            tempCamera.orthographic = true;
            tempCamera.orthographicSize = 0.5f;
            tempCamera.farClipPlane = 10.0f;
            tempCamera.nearClipPlane = 0.1f;
            tempCamera.clearFlags = CameraClearFlags.Color;
            tempCamera.backgroundColor = Color.clear;
            tempCamera.renderingPath = RenderingPath.Forward;
            tempCamera.useOcclusionCulling = false;
            tempCamera.allowMSAA = false;
            tempCamera.allowHDR = true;
            
            int previewLayer = 31;
            tempCamera.cullingMask = 1 << previewLayer;
            tempCamera.targetTexture = renderTexture;
            
            // Lighting setup
            Light[] sceneLights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);
            foreach (var light in sceneLights)
            {
                light.enabled = false;
            }
            
            Light tempLight = tempCameraObj.AddComponent<Light>();
            tempLight.type = LightType.Directional;
            tempLight.color = Color.white;
            tempLight.intensity = 1.0f;
            tempLight.cullingMask = 1 << previewLayer;
            
            var tempQuad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            tempQuad.GetComponent<MeshRenderer>().sharedMaterial = material;
            tempQuad.layer = previewLayer;
            
            // Render each cubemap face
            CubemapFace[] faces = new CubemapFace[]
            {
                CubemapFace.PositiveX, CubemapFace.NegativeX,
                CubemapFace.PositiveY, CubemapFace.NegativeY,
                CubemapFace.PositiveZ, CubemapFace.NegativeZ
            };
            
            foreach (var face in faces)
            {
                // Set the shader property for which cubemap face to render
                material.SetInt(s_cubemapFace, (int)face);
                
                tempCamera.Render();
                
                RenderTexture.active = renderTexture;
                Texture2D tempTex = new Texture2D(faceSize, faceSize, TextureFormat.ARGB32, false);
                tempTex.ReadPixels(new Rect(0, 0, faceSize, faceSize), 0, 0);
                tempTex.Apply();
                
                // Copy to cubemap face
                cubemap.SetPixels(tempTex.GetPixels(), face);
                
                Object.DestroyImmediate(tempTex);
            }
            
            cubemap.Apply();
            
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(renderTexture);
            Object.DestroyImmediate(tempCameraObj);
            Object.DestroyImmediate(tempQuad);
            
            foreach (var light in sceneLights)
            {
                light.enabled = true;
            }
            
            return cubemap;
        }
        
        public static void SaveCubemapTexture(Cubemap cubemap, string savePath)
        {
            var tex = CubemapToTexture2D(cubemap);

            TextureUtilities.SaveTextureAsPNG(savePath,tex);
            
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(savePath);
            Object.DestroyImmediate(cubemap);
        }

        public static Texture2D CubemapToTexture2D(Cubemap cubemap)
        {
            int faceSize = cubemap.width; 
            int layoutWidth = faceSize * 4;
            int layoutHeight = faceSize * 3;
            
            Texture2D tex = new Texture2D(layoutWidth, layoutHeight, cubemap.format, false);
    
            // Fill the texture in a horizontal cross layout:
            //      +----+----+----+----+
            //      |    | +Y |    |    |
            //      +----+----+----+----+
            //      | -X | +Z | +X | -Z |
            //      +----+----+----+----+
            //      |    | -Y |    |    |
            //      +----+----+----+----+
            
            CopyCubemapFaceToTexture(cubemap, CubemapFace.NegativeX, tex, faceSize, 0, faceSize);
            CopyCubemapFaceToTexture(cubemap, CubemapFace.NegativeY, tex, faceSize, faceSize, 0);
            CopyCubemapFaceToTexture(cubemap, CubemapFace.PositiveZ, tex, faceSize, faceSize, faceSize);
            CopyCubemapFaceToTexture(cubemap, CubemapFace.PositiveY, tex, faceSize, faceSize, faceSize * 2);
            CopyCubemapFaceToTexture(cubemap, CubemapFace.PositiveX, tex, faceSize, faceSize * 2, faceSize);
            CopyCubemapFaceToTexture(cubemap, CubemapFace.NegativeZ, tex, faceSize,  faceSize * 3, faceSize);
            return tex;
        }

        public static void CopyCubemapFaceToTexture(Cubemap cubemap, CubemapFace face, Texture2D texture, int faceSize, int offsetX, int offsetY)
        {
            // we need to flip our textures on Y before saving
            for (int y = 0; y < faceSize; y++)
            {
                for (int x = 0; x < faceSize; x++)
                {
                    texture.SetPixel(offsetX + x, offsetY + (faceSize - 1 - y), cubemap.GetPixel(face, x, y));
                }
            }
        }
    }
}