using System.Collections.Generic;
using UnityEngine;
using RPGWorldLLM.Utils;

namespace RPGWorldLLM.GenAI.Story
{

    public class CharacterDefinition : HistoryStoryObject
    {
        public Dictionary<string, string> parameters = new Dictionary<string, string>();

        public static CharacterDefinition FromString(string str)
        {
            Dictionary<string, string> dict = TextProcessor.ToDictionary(str);

            var character = new CharacterDefinition
            {
                parameters = new Dictionary<string, string>()
            };

            foreach (KeyValuePair<string, string> kvp in dict)
            {
                string key = kvp.Key?.Trim().ToLower();
                string value = kvp.Value?.Trim();

                if (string.IsNullOrEmpty(key))
                    continue;

                if (key == "name" || key == "narrator")
                {
                    character.name = value;
                    continue;
                }

                if (key == "physical description")
                {
                    character.description = value;
                    continue;
                }

                character.parameters[kvp.Key.Trim()] = value;
            }

            return character;
        }
        
        public override BaseStoryObject Clone()
        {
            var clone = new CharacterDefinition();

            // Copy BaseStoryObject properties
            clone.id = id;
            clone.name = name;
            clone.description = description;

            // Deep copy HistoryStoryObject lists
            clone.memoryItems = new List<MemoryItem>();
            foreach (var item in memoryItems)
            {
                clone.memoryItems.Add(new MemoryItem
                {
                    rawText = item.rawText,
                    summmarizedText = item.summmarizedText
                });
            }

            clone.factItems = new List<FactItem>();
            foreach (var item in factItems)
            {
                clone.factItems.Add(new FactItem
                {
                    name = item.name,
                    text = item.text
                });
            }

            // Deep copy parameters dictionary
            clone.parameters = new Dictionary<string, string>();
            if (parameters != null)
            {
                foreach (var kvp in parameters)
                {
                    clone.parameters[kvp.Key] = kvp.Value;
                }
            }

            return clone;
        }
    }

}

