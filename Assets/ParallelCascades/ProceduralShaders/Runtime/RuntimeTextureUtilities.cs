using UnityEngine;

namespace ParallelCascades.ProceduralShaders.Editor
{
    public static class RuntimeTextureUtilities
    {
        public static void SetTextureFromGradient(Gradient gradient, Texture2D texture)
        {
            int resolution = texture.width;
            for (int i = 0; i < resolution; i++)
            {
                float t = i / (float)(resolution - 1);
                Color color = gradient.Evaluate(t);
                texture.SetPixel(i, 0, color);
            }

            texture.Apply();
        }
    }
}