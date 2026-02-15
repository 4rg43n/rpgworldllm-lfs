using System.Collections.Generic;
using System.Text;
using RPGWorldLLM.Logging;
using RPGWorldLLM.Utils;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    public class StoryDefinition : HistoryStoryObject
    {
        public Dictionary<string, List<string>> sectionMap = new Dictionary<string, List<string>>(); // the sections after they're processed
        
        public Dictionary<string, List<string>> inputSectionMap = new Dictionary<string, List<string>>(); // raw section data

        public override BaseStoryObject Clone()
        {
            StoryDefinition newStory = new StoryDefinition();

            // Copy BaseStoryObject properties
            newStory.id = id;
            newStory.name = name;
            newStory.description = description;

            // Deep copy HistoryStoryObject lists
            newStory.memoryItems = new List<MemoryItem>();
            foreach (var item in memoryItems)
            {
                newStory.memoryItems.Add(new MemoryItem
                {
                    rawText = item.rawText,
                    summmarizedText = item.summmarizedText
                });
            }

            newStory.factItems = new List<FactItem>();
            foreach (var item in factItems)
            {
                newStory.factItems.Add(new FactItem
                {
                    name = item.name,
                    text = item.text
                });
            }

            // Deep copy sectionMap
            newStory.sectionMap = new Dictionary<string, List<string>>();
            foreach (var kvp in sectionMap)
            {
                newStory.sectionMap[kvp.Key] = new List<string>(kvp.Value);
            }

            // Deep copy inputSectionMap
            newStory.inputSectionMap = new Dictionary<string, List<string>>();
            foreach (var kvp in inputSectionMap)
            {
                newStory.inputSectionMap[kvp.Key] = new List<string>(kvp.Value);
            }

            return newStory;
        }

        public List<BaseStoryObject> ReadStoryObjectsFromInputMap()
        {
            List<BaseStoryObject> storyObjects = new List<BaseStoryObject>();
            
            foreach (KeyValuePair<string, List<string>> kvp in inputSectionMap)
            {
                if (kvp.Key.Trim().ToLower() == "characters")
                {
                    foreach (string str in kvp.Value)
                    {
                        string cstr = TextProcessor.ReplaceAuto("{{char}}", str);
                        CharacterDefinition chr = CharacterDefinition.FromString(cstr);
                        storyObjects.Add(chr);
                    }
                }
            }

            return storyObjects;
        }

        public static StoryDefinition LoadFromTextAssetST(string resourcePath)
        {
            StoryDefinition story = new StoryDefinition();

            TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
            if (textAsset == null)
            {
                Debug.LogError($"StoryDefinition: Could not load TextAsset at Resources/{resourcePath}");
                return story;
            }

            string[] lines = textAsset.text.Split('\n');

            string currentSection = null;
            List<string> currentBlockLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].TrimEnd('\r');

                if (line.StartsWith("//") || line.StartsWith("###"))
                    continue;

                if (line.StartsWith("##"))
                {
                    // finalize previous section's last block
                    if (currentSection != null && currentBlockLines.Count > 0)
                    {
                        story.inputSectionMap[currentSection].Add(string.Join("\n", currentBlockLines));
                        currentBlockLines.Clear();
                    }

                    string headerContent = line.Substring(3).Trim();
                    currentSection = headerContent.Split(' ')[0];
                    story.inputSectionMap[currentSection] = new List<string>();
                    continue;
                }

                if (currentSection == null)
                    continue;

                if (string.IsNullOrWhiteSpace(line))
                {
                    if (currentBlockLines.Count > 0)
                    {
                        story.inputSectionMap[currentSection].Add(string.Join("\n", currentBlockLines));
                        currentBlockLines.Clear();
                    }
                }
                else
                {
                    currentBlockLines.Add(line);
                }
            }

            // finalize last block
            if (currentSection != null && currentBlockLines.Count > 0)
            {
                story.inputSectionMap[currentSection].Add(string.Join("\n", currentBlockLines));
            }

            foreach (string key in  story.inputSectionMap.Keys)
            {
                StringBuilder sb =  new StringBuilder();
                List<string> blockLines = story.inputSectionMap[key];
                foreach (string line in blockLines)
                {
                    sb.AppendLine(line);
                    sb.Append("\n");
                }

                StoryLogData log = new StoryLogData(key, sb.ToString());
                LogManager.Instance.Log(log);
            }

            return story;
        }
    }

}
