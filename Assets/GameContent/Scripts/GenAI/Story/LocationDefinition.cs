using System.Collections.Generic;
using UnityEngine;
using RPGWorldLLM.Utils;

namespace RPGWorldLLM.GenAI.Story
{

    public class LocationDefinition : HistoryStoryObject
    {
        public override string PrintName
        {
            get { return name+" (Location)"; }
        }

        public static LocationDefinition FromString(string str)
        {
            Dictionary<string, string> dict = TextProcessor.ToDictionary(str);

            var character = new LocationDefinition
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
    }
}

