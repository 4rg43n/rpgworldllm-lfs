using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGWorldLLM.Logging;
using RPGWorldLLM.Utils;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    public class StoryDefinition : HistoryStoryObject
    {
        public Dictionary<string, string> settings = new Dictionary<string, string>();
        public Dictionary<string, List<string>> sectionMap = new Dictionary<string, List<string>>(); // the sections after they're processed
        
        public Dictionary<string, List<List<string>>> inputSectionMap = new Dictionary<string, List<List<string>>>(); // raw section data

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
            newStory.inputSectionMap = new Dictionary<string, List<List<string>>>();
            foreach (var kvp in inputSectionMap)
            {
                List<List<string>> block = new List<List<string>>();
                foreach (List<string> line in kvp.Value)
                {
                    List<string> lineCopy = new List<string>(line);
                    block.Add(lineCopy);
                }
                
                newStory.inputSectionMap[kvp.Key] = block;
            }

            return newStory;
        }

        public List<BaseStoryObject> ProcessSections()
        {
            List<BaseStoryObject> storyObjects = new List<BaseStoryObject>();
            
            foreach (string section in inputSectionMap.Keys)
            {
                if (section == "title")
                    name = string.Join("\n", inputSectionMap[section]);
                else if (section == "description")
                    description = string.Join("\n", inputSectionMap[section]);
                else if (section == "characters")
                {
                    List<List<string>> charDefs = inputSectionMap[section];
                    foreach (List<string> charDef in charDefs)
                    {
                        string charStr = string.Join("\n", charDef);
                        CharacterDefinition character = CharacterDefinition.FromString(charStr);
                        storyObjects.Add(character);
                    }
                }
                else if (section == "locations")
                {
                    List<List<string>> locDefs = inputSectionMap[section];
                    foreach (List<string> locDef in locDefs)
                    {
                        string locStr = string.Join("\n", locDef);
                        LocationDefinition location = LocationDefinition.FromString(locStr);
                        storyObjects.Add(location);
                    }
                }
                else
                {
                    string desc = "";
                    foreach (List<string> line in inputSectionMap[section])
                    {
                        foreach (string lstr in line)
                        {
                            desc += lstr.Trim() + "\n";
                        }

                        desc += "\n";
                    }

                    desc = desc.Trim();
                    parameters[section] = desc;
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

            string currentSection = "";
            List<string> currentBlockLines = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();

                if (line.StartsWith("//") || line.StartsWith("###"))
                    continue;

                if (line.StartsWith("##"))
                {
                    if (currentSection!="" && currentBlockLines.Count>0)
                    {
                        // end black
                        story.inputSectionMap[currentSection.Trim().ToLower()] = FormatBlock(currentBlockLines);
                        currentSection = "";
                        currentBlockLines.Clear();
                    }

                    // start a new block
                    string[] header_tokens = line.Split(" ");
                    currentSection = header_tokens[1];
                    currentBlockLines.Clear();
                }
                else
                {
                    // add line to current block
                    if (currentSection!="")
                    {
                        currentBlockLines.Add(line);
                    }
                }
            }
            
            // make sure last block is added
            if (currentSection!=""&&currentBlockLines.Count>0)
                story.inputSectionMap[currentSection.Trim().ToLower()] = FormatBlock(currentBlockLines);

            return story;
        }

        static List<List<string>> FormatBlock(List<string> block)
        {
            List<List<string>> formatted = new List<List<string>>();
            List<string> inner_block = null;

            foreach (string line in block)
            {
                if (line != "")
                {
                    if (inner_block==null)
                        inner_block = new List<string>();
                    inner_block.Add(line);
                }
                else
                {
                    if (inner_block!=null)
                    {
                        formatted.Add(inner_block);
                        inner_block = null;
                    }
                }
            }
            
            if (inner_block!=null)
                formatted.Add(inner_block);

            return formatted;
        }

    }

}
