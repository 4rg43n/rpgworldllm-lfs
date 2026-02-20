using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RPGWorld.UI
{
    public class ChatMessageTextUI : ChatMessageUI
    {
        private TextMeshProUGUI textComponent;

        public static ChatMessageTextUI CreateST(Transform parent, GameObject prefab, string text)
        {
            var go = Instantiate(prefab, parent);
            go.name = "ChatMsg_Text";

            var tmp = go.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = text;

            var ui = go.AddComponent<ChatMessageTextUI>();
            ui.textComponent = tmp;

            
            return ui;
        }
    }
}
