using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public abstract class HistoryStoryObject : BaseStoryObject
    {
        public List<MemoryItem> memoryItems = new List<MemoryItem>();
        public List<FactItem> factItems = new List<FactItem>();
        
        public Dictionary<string, string> parameters = new Dictionary<string, string>();

        public void FormatText(PlayerDefinition player)
        {
            name = name.Replace("{{user}}", player.tag); // name of the story object
            tag = tag.Replace("{{user}}", player.tag); // tag of the story object
            description = description.Replace("{{user}}", player.tag); // description of the story object
            
            List<string> keys = new List<string>(parameters.Keys);

            foreach (string key in keys)
            {
                string str = parameters[key];
                str = str.Replace("{{user}}", player.tag);
                parameters[key] = str;
            }
        }

        public override void CopyFrom(BaseStoryObject other)
        {
            if (other is HistoryStoryObject otherHistory)
            {
                base.CopyFrom(other);
                // Deep copy memoryItems
                memoryItems = new List<MemoryItem>();
                foreach (var item in otherHistory.memoryItems)
                {
                    memoryItems.Add(new MemoryItem(item.rawText, item.summmarizedText));
                }

                // Deep copy factItems
                factItems = new List<FactItem>();
                foreach (var item in otherHistory.factItems)
                {
                    factItems.Add(new FactItem { name = item.name, text = item.text });
                }
                
            }
            else 
                Debug.LogError("Cannot copy from " + other.GetType().Name);
        }

    }

    public class FactItem
    {
        public string name;
        public string text;
    }

    public class MemoryItem
    {
        public string rawText;
        public string summmarizedText;

        public MemoryItem(string response)
        {
            rawText=summmarizedText=response;
        }
        
        public MemoryItem(string response, string summary)
        {
            rawText=response;
            summmarizedText=summary;
        }
    }
}
