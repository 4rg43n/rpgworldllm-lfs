using System;
using UnityEngine;

namespace RPGWorld.Chat
{
    [Serializable]
    public abstract class ChatMessage
    {
        public string sender;

        public ChatMessage(string sender)
        {
            this.sender = sender;
        }
    }

    [Serializable]
    public class ChatMessageText : ChatMessage
    {
        public string text;

        public ChatMessageText(string sender, string text) : base(sender)
        {
            this.sender = sender;
            this.text = text;
        }
    }

    [Serializable]
    public class ChatMessageImage : ChatMessage
    {
        [NonSerialized] public Texture2D image;

        public ChatMessageImage(string sender, Texture2D image) : base(sender)
        {
            this.image = image;
        }
    }
}
