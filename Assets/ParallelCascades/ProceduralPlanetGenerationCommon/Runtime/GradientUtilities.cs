using UnityEngine;

namespace ParallelCascades.ProceduralPlanetGenerationCommon.Runtime
{
    public static class GradientUtilities
    {
        public static Gradient RandomMountainGradient(Color mainColor)
        {
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(Color.Lerp(mainColor, Color.black, Random.Range(0.2f, 0.6f)), 0);
            colorKeys[1] = new GradientColorKey(Color.white, 1);
            gradient.colorKeys = colorKeys;
            return gradient;
        }

        public static Gradient RandomLandColorGradient(Color mainColor)
        {
            Gradient gradient = new Gradient(); 
            GradientColorKey[] colorKeys = new GradientColorKey[4];
            colorKeys[0] = new GradientColorKey(mainColor, 0);
        
            // lighter variation
            Color secondShade = Color.Lerp(mainColor, Color.white, Random.Range(0.2f, 0.4f));
            colorKeys[1] = new GradientColorKey(secondShade, Random.Range(0.2f, 0.45f));
        
            // darker shade
            Color thirdShade = Color.Lerp(mainColor, Color.black, Random.Range(0.3f, 0.6f));
            colorKeys[2] = new GradientColorKey(thirdShade, Random.Range(0.5f, 0.9f));
        
            // End with the lightest shade
            Color lightShade = Color.Lerp(mainColor, Color.white, Random.Range(0.5f, 0.7f));
            colorKeys[3] = new GradientColorKey(lightShade, 1);
            gradient.colorKeys = colorKeys;
            return gradient;
        }
    }
}