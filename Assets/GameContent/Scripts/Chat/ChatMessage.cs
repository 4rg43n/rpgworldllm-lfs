using System;
using UnityEngine;

namespace RPGWorld.Chat
{
    [Serializable]
    public abstract class ChatMessage
    {
    }

    [Serializable]
    public class ChatMessageText : ChatMessage
    {
        public string sender;
        public string text;

        public ChatMessageText(string sender, string text)
        {
            this.sender = sender;
            this.text = text;
        }
    }

    [Serializable]
    public class ChatMessageImage : ChatMessage
    {
        [NonSerialized] public Texture2D image;

        public ChatMessageImage(Texture2D image)
        {
            this.image = image;
        }
    }
}
