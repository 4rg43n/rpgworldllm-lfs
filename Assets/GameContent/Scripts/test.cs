using UnityEngine;

/// <summary>
/// Creates a red square sprite at world origin and rotates it continuously.
/// </summary>
public class test : MonoBehaviour
{
    /// <summary>
    /// Initializes the red square sprite at position (0,0,0).
    /// Creates a 100x100 pixel red texture, converts it to a sprite,
    /// and attaches it to the GameObject via SpriteRenderer.
    /// </summary>
    void Start()
    {
        // Create a 100x100 red square texture
        Texture2D texture = new Texture2D(100, 100);
        Color[] pixels = new Color[100 * 100];

        // Fill all pixels with red color
        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = Color.red;
        }

        // Apply pixel data to texture
        texture.SetPixels(pixels);
        texture.Apply();

        // Create sprite from texture with center pivot
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f));

        // Add SpriteRenderer component and assign the sprite
        SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprite;

        // Position GameObject at world origin
        transform.position = Vector3.zero;
    }

    /// <summary>
    /// Rotates the GameObject around the Z-axis each frame.
    /// Rotation speed is 90 degrees per second.
    /// </summary>
    void Update()
    {
        // Rotate 90 degrees per second around Z axis
        transform.Rotate(0, 0, 90 * Time.deltaTime);
    }
}
