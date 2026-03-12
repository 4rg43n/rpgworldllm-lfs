using System;
using System.Collections.Generic;
using UnityEngine;
using RPGWorldLLM.Utils;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class CharacterDefinition : HistoryStoryObject
    {

        public bool IsNarrator()
        {
            if (Parameter(KEY_TITLE_NAME).Trim().ToLower().Contains(TAG_NARRATOR)||
                Parameter(KEY_TAG).Trim().ToLower().Contains(TAG_NARRATOR))
                return true;
            
            return false;
        }
        
        public override BaseStoryObject Clone()
        {
            var clone = new CharacterDefinition();
            clone.CopyFrom(this);
            return clone;
        }

        public static CharacterDefinition FromString(string str)
        {
            Dictionary<string, string> dict = TextProcessor.ToDictionary(str);

            var character = new CharacterDefinition();

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

