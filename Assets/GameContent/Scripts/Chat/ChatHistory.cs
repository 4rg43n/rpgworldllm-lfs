using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPGWorld.UI;
using TMPro;

namespace RPGWorld.Chat
{
    public class ChatHistory : MonoBehaviour
    {
        public static ChatHistory Instance { get; private set; }

        [SerializeField] private ScrollRect scrollRect;

        [SerializeField] private GameObject textMessagePrefab;
        [SerializeField] private GameObject imageMessagePrefab;

        private const string TextPrefabPath = "Assets/GameContent/Prefabs/UI/TextMsgPanel_Player.prefab";
        private const string ImagePrefabPath = "Assets/GameContent/Prefabs/UI/ImageMsgPanel.prefab";
        [SerializeField] private Transform content;

        public Color playerColor = Color.gray;
        public Color narratorColor = Color.red;
        
        public Color playerTextColor = Color.white;
        public Color narratorTextColor = Color.white;

        private readonly List<ChatMessage> messages = new List<ChatMessage>();

        public IReadOnlyList<ChatMessage> Messages => messages;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(gameObject);

            LoadPrefabs();

            if (scrollRect == null)
            {
                var chatView = GameObject.Find("Chat View");
                if (chatView != null)
                    scrollRect = chatView.GetComponent<ScrollRect>();
            }

            if (scrollRect != null)
            {
                WireScrollRect();

                if (content == null)
                    content = scrollRect.content;
            }
        }

        private void LoadPrefabs()
        {
#if UNITY_EDITOR
            if (textMessagePrefab == null)
                textMessagePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(TextPrefabPath);
            if (imageMessagePrefab == null)
                imageMessagePrefab = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(ImagePrefabPath);
#endif
        }


        private void WireScrollRect()
        {
            if (scrollRect.viewport == null)
            {
                var viewport = scrollRect.transform.Find("Viewport");
                if (viewport != null)
                    scrollRect.viewport = viewport.GetComponent<RectTransform>();
            }

            if (scrollRect.content == null)
            {
                var viewport = scrollRect.viewport;
                if (viewport != null)
                {
                    var contentTransform = viewport.Find("Content");
                    if (contentTransform != null)
                        scrollRect.content = contentTransform.GetComponent<RectTransform>();
                }
            }
        }


        public void AddText(string sender, string text)
        {
            ChatMessageText message = new ChatMessageText(sender, text);
            messages.Add(message);
            AppendMessageUI(message);
        }

        public void AddImage(string sender, Texture2D image)
        {
            ChatMessageImage message = new ChatMessageImage(sender, image);
            messages.Add(message);
            AppendMessageUI(message);
        }

        public void RebuildUI()
        {
            for (int i = content.childCount - 1; i >= 0; i--)
                Destroy(content.GetChild(i).gameObject);

            foreach (var message in messages)
                AppendMessageUI(message);
        }

        private void AppendMessageUI(ChatMessage message)
        {
            switch (message)
            {
                case ChatMessageText textMsg:
                    ChatMessageTextUI textUI = ChatMessageTextUI.CreateST(content, textMessagePrefab, textMsg.text);
                    SetMessageColor(message, textUI);
                    break;
                case ChatMessageImage imageMsg:
                    ChatMessageImageUI imgUI = ChatMessageImageUI.CreateST(content, imageMessagePrefab, imageMsg.image);
                    SetMessageColor(message, imgUI);
                    break;
            }

            ScrollToBottom();
        }

        private void SetMessageColor(ChatMessage message, ChatMessageUI ui)
        {
            if (message.sender == "narrator")
            {
                ui.gameObject.GetComponent<Image>().color = narratorColor;
                ui.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = narratorTextColor;
            }
            else if (message.sender == "player")
            {
                ui.gameObject.GetComponent<Image>().color = playerColor;
                ui.gameObject.GetComponentInChildren<TextMeshProUGUI>().color = playerTextColor;
            }
        }

        private float GetScrollViewWidth()
        {
            if (scrollRect != null)
                return scrollRect.GetComponent<RectTransform>().rect.width;
            return 400f;
        }

        private void ScrollToBottom()
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.normalizedPosition = Vector2.zero;
        }
    }
}
