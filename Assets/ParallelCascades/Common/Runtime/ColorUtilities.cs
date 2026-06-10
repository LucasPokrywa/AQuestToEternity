using UnityEngine;

namespace ParallelCascades.Common.Runtime
{
    public static class ColorUtilities
    {
        public static Gradient CreateRandomGradientFromColor(Color color, int stepsMin = 2, int stepsMax = 8, float saturationMin = 0.5f, float saturationMax = 3f)
        {
            Gradient gradient = new Gradient();
            
            // Unity gradients have a max of 8 color keys.
            int steps = Random.Range(stepsMin, stepsMax);
            GradientColorKey[] colorKeys = new GradientColorKey[steps];
            
            for (int i = 0; i < steps; i++)
            {
                float time = i / (float)(steps - 1);
                Color gradientColor = color * Random.Range(saturationMin, saturationMax);
                colorKeys[i] = new GradientColorKey(gradientColor, time);
            }
            gradient.colorKeys = colorKeys;
            return gradient;
        }
        
        public static Gradient CreateRandomGradientFromTwoColors(Color colorA, Color colorB, int stepsMin = 2, int stepsMax = 8, float saturationMin = 0.5f, float saturationMax = 3f)
        {
            Gradient gradient = new Gradient();
            
            int steps = Random.Range(stepsMin, stepsMax);
            GradientColorKey[] colorKeys = new GradientColorKey[steps];
            
            for (int i = 0; i < steps; i++)
            {
                float t = i / (float)(steps - 1);
                Color lerped = Color.Lerp(colorA, colorB, t); // generate a color between colorA and colorB
                Color gradientColor = lerped * Random.Range(saturationMin, saturationMax);
                colorKeys[i] = new GradientColorKey(gradientColor, t);
            }
            gradient.colorKeys = colorKeys;
            return gradient;
        }

        public static Gradient GenerateGradientSliceFromGradient(Gradient sampleGradient, float stepRange)
        {
            float t = Random.Range(0f, 1f);
            float step = Random.Range(-stepRange, stepRange);
            
            Gradient gradient = new Gradient();
            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(sampleGradient.Evaluate(t), 0);
            colorKeys[1] = new GradientColorKey(sampleGradient.Evaluate(Mathf.Clamp01(t + step)), 1);
            gradient.colorKeys = colorKeys;
            return gradient;
        }

        public static Color RandomColorSaturationFromGradient(Gradient gradient)
        {
            Color baseColor = gradient.Evaluate(Random.Range(0f, 1f));
            Color.RGBToHSV(baseColor, out float h, out float s, out float v);
            s = Random.Range(0f, 1f);
            return Color.HSVToRGB(h, s, v);
        }
    }
}