using System;
using System.Collections.Generic;
using UnityEngine;
using RPGWorldLLM.Utils;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class CharacterDefinition : HistoryStoryObject
    {
        public bool isNarrator = false;

        public override string PrintName
        {
            get => name +(isNarrator ? " (Narrator)" : " (Character)");
        }

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

                if (key == "narrator")
                {
                    character.name = value;
                    character.isNarrator = true;
                    continue;
                }

                if (key == "name")
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
    }

}

