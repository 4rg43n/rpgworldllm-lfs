using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class HistoryStoryObject : BaseStoryObject
    {
        public List<MemoryItem> memoryItems = new List<MemoryItem>();
        public List<FactItem> factItems = new List<FactItem>();
        
        public Dictionary<string, string> parameters = new Dictionary<string, string>();
        
        public override BaseStoryObject Clone()
        {
            var clone = new HistoryStoryObject();

            // Copy base properties
            clone.id = id;
            clone.name = name;
            clone.description = description;

            // Deep copy memoryItems
            clone.memoryItems = new List<MemoryItem>();
            foreach (var item in memoryItems)
            {
                clone.memoryItems.Add(new MemoryItem(item.rawText, item.summmarizedText));
            }

            // Deep copy factItems
            clone.factItems = new List<FactItem>();
            foreach (var item in factItems)
            {
                clone.factItems.Add(new FactItem
                {
                    name = item.name,
                    text = item.text
                });
            }

            return clone;
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
