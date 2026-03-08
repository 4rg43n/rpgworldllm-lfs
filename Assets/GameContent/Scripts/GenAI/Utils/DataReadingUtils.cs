using System.Collections.Generic;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Utils
{
    public class DataReadingUtils : MonoBehaviour
    {
        /// <summary>
        /// Loads a text asset from a Resources directory and returns its lines as a list.
        /// Blank lines are included as empty strings. Each line is trimmed.
        /// </summary>
        /// <param name="resourcePath">Path relative to a Resources folder, without extension.</param>
        public static Dictionary<string, List<List<string>>> ReadStoryDefinitionST(string resourcePath)
        {
            List<string> lines = ReadTextAssetLinesST(resourcePath);
            var result = new Dictionary<string, List<List<string>>>();

            string currentSection = null;
            List<string> currentRecord = null;

            foreach (string line in lines)
            {
                // Rule 1: skip comment lines
                if (line.StartsWith("###") || line.StartsWith("//"))
                    continue;

                // Rule 2: section header
                if (line.StartsWith("##"))
                {
                    // Flush any in-progress record before switching sections
                    if (currentSection != null && currentRecord != null && currentRecord.Count > 0)
                    {
                        result[currentSection].Add(currentRecord);
                        currentRecord = null;
                    }

                    string sectionName = line.Substring(2).Trim().Split(' ')[0].Trim().ToLower();
                    if (!string.IsNullOrEmpty(sectionName))
                    {
                        currentSection = sectionName;
                        if (!result.ContainsKey(currentSection))
                            result[currentSection] = new List<List<string>>();
                    }
                    continue;
                }

                if (currentSection == null)
                    continue;

                // Rule 3: blank line ends the current record
                if (string.IsNullOrEmpty(line))
                {
                    if (currentRecord != null && currentRecord.Count > 0)
                    {
                        result[currentSection].Add(currentRecord);
                        currentRecord = null;
                    }
                }
                else
                {
                    if (currentRecord == null)
                        currentRecord = new List<string>();
                    currentRecord.Add(line);
                }
            }

            // Flush final record
            if (currentSection != null && currentRecord != null && currentRecord.Count > 0)
                result[currentSection].Add(currentRecord);

            return result;
        }

        public static List<string> ReadTextAssetLinesST(string resourcePath)
        {
            TextAsset textAsset = Resources.Load<TextAsset>(resourcePath);
            if (textAsset == null)
            {
                Debug.LogError($"DataReadingUtils: Could not load TextAsset at Resources path '{resourcePath}'");
                return new List<string>();
            }

            string[] rawLines = textAsset.text.Split('\n');
            List<string> lines = new List<string>(rawLines.Length);
            foreach (string line in rawLines)
            {
                lines.Add(line.Trim());
            }
            return lines;
        }
    }
}


