using System;
using System.Collections.Generic;
using UnityEngine;
using RPGWorldLLM.Utils;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class LocationDefinition : HistoryStoryObject
    {

        public override BaseStoryObject Clone()
        {
            LocationDefinition newLocation = new LocationDefinition();
            newLocation.CopyFrom(this);

            return newLocation;
        }

        public static LocationDefinition FromString(string str)
        {
            Dictionary<string, string> dict = TextProcessor.ToDictionary(str);

            var character = new LocationDefinition();

            foreach (KeyValuePair<string, string> kvp in dict)
            {
                string key = kvp.Key?.Trim().ToLower();
                string value = kvp.Value?.Trim();

                if (string.IsNullOrEmpty(key))
                    continue;

                if (key == TAG_NAME || key == TAG_NARRATOR)
                {
                    character.objectParameters[KEY_TITLE_NAME] = value;
                    continue;
                }

                if (key == TAG_PHYSICAL_DESCRIPTION)
                {
                    character.objectParameters[KEY_DESC] = value;
                    continue;
                }

                character.objectParameters[kvp.Key.Trim().ToLower()] = value;
            }

            return character;
        }
    }
}

