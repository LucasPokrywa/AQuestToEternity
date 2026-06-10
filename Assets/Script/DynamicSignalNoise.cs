using UnityEngine;
using UnityEngine.UI;

public class DynamicSignalNoise : MonoBehaviour
{
    public Image targetImage;
    public int textureSize = 512;
    [Range(0f, 1f)] public float intensity = 0f;

    private Texture2D texture;
    private Color[] pixels;

    void Start()
    {
        texture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
        texture.filterMode = FilterMode.Point;

        pixels = new Color[textureSize * textureSize];

        targetImage.sprite = Sprite.Create(
            texture,
            new Rect(0, 0, textureSize, textureSize),
            new Vector2(0.5f, 0.5f)
        );

        SetAlpha(0f);
    }

    void Update()
    {
        if (intensity <= 0.01f)
        {
            SetAlpha(0f);
            return;
        }

        for (int y = 0; y < textureSize; y++)
        {
            bool strongLine = Random.value < intensity * 0.08f;

            for (int x = 0; x < textureSize; x++)
            {
                float v = Random.value;

                if (strongLine)
                    v = Random.Range(0.7f, 1f);

                float a = Random.Range(0.05f, 0.35f) * intensity;

                pixels[y * textureSize + x] = new Color(v, v, v, a);
            }
        }

        texture.SetPixels(pixels);
        texture.Apply();

        SetAlpha(Mathf.Lerp(0f, 0.75f, intensity));
    }

    void SetAlpha(float alpha)
    {
        Color c = targetImage.color;
        c.a = alpha;
        targetImage.color = c;
    }
}