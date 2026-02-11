using UnityEngine;
using UnityEngine.UI;

namespace RPGWorld.UI
{
    public class ChatMessageImageUI : MonoBehaviour
    {
        private Image imageComponent;

public static ChatMessageImageUI CreateST(Transform parent, GameObject prefab, Texture2D texture)
        {
            var go = Instantiate(prefab, parent);
            go.name = "ChatMsg_Image";

            var childImage = go.transform.Find("Image");
            var image = childImage.GetComponent<Image>();

            var sprite = Sprite.Create(
                texture,
                new Rect(0, 0, texture.width, texture.height),
                new Vector2(0.5f, 0.5f));
            image.sprite = sprite;

            var ui = go.AddComponent<ChatMessageImageUI>();
            ui.imageComponent = image;

            return ui;
        }
    }
}
