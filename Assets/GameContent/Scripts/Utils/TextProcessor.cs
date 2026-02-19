using System;
using System.Collections.Generic;

namespace RPGWorldLLM.Utils
{
    public static class TextProcessor
    {
        public static string Replace(string toReplace, string replaceWith, string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(toReplace))
                return str;

            return str.Replace(toReplace, replaceWith ?? string.Empty);
        }

        public static string ReplaceAuto(string toReplace, string str, out string replaceWithTag)
        {
            replaceWithTag = "";
            
            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(toReplace))
                return str;

            // Look for a definition line in the form: toReplace=replaceWith
            // Example: "{{char}}=Coffin Hill"
            string replaceWith = null;

            string normalized = str.Replace("\r\n", "\n").Replace('\r', '\n');
            string[] lines = normalized.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                int eqIndex = line.IndexOf('=');
                if (eqIndex < 0)
                    continue;

                string left = line.Substring(0, eqIndex).Trim();
                if (!string.Equals(left, toReplace, StringComparison.Ordinal))
                    continue;

                replaceWith = line.Substring(eqIndex + 1).Trim();
                lines[i] = null; // remove the definition line
                break;
            }

            if (replaceWith == null)
                return str;
            
            replaceWithTag = replaceWith;

            string withoutDefinition = string.Join("\n", Array.FindAll(lines, l => l != null));
            return Replace(toReplace, replaceWith, withoutDefinition);
        }

        public static Dictionary<string, string> ToDictionary(string str)
        {
            var result = new Dictionary<string, string>();

            if (string.IsNullOrWhiteSpace(str))
                return result;

            string normalized = str.Replace("\r\n", "\n").Replace('\r', '\n');
            string[] lines = normalized.Split('\n');

            static bool IsKeyLine(string line)
            {
                if (string.IsNullOrWhiteSpace(line))
                    return false;

                string trimmed = line.Trim();
                return trimmed.EndsWith("-", StringComparison.Ordinal) || trimmed.Contains(":");
            }

            static bool TryParseKeyValueLine(string line, out string key, out string value)
            {
                key = null;
                value = null;

                if (string.IsNullOrWhiteSpace(line))
                    return false;

                int colonIndex = line.IndexOf(':');
                if (colonIndex < 0)
                    return false;

                key = line.Substring(0, colonIndex).Trim();
                value = line.Substring(colonIndex + 1).Trim();

                return !string.IsNullOrEmpty(key);
            }

            static bool TryParseBlockKeyLine(string line, out string key)
            {
                key = null;

                if (string.IsNullOrWhiteSpace(line))
                    return false;

                string trimmed = line.Trim();
                if (!trimmed.EndsWith("-", StringComparison.Ordinal))
                    return false;

                key = trimmed.Substring(0, trimmed.Length - 1).Trim();
                return !string.IsNullOrEmpty(key);
            }

            int i = 0;
            while (i < lines.Length)
            {
                string line = lines[i];

                if (TryParseKeyValueLine(line, out string key, out string value))
                {
                    result[key] = value;
                    i++;
                    continue;
                }

                if (TryParseBlockKeyLine(line, out key))
                {
                    i++; // move to the first line of the block value

                    var blockLines = new List<string>();
                    while (i < lines.Length && !IsKeyLine(lines[i]))
                    {
                        blockLines.Add(lines[i]);
                        i++;
                    }

                    // Preserve internal newlines, but remove trailing blank lines/newlines
                    string blockValue = string.Join("\n", blockLines).TrimEnd('\n', '\r');
                    result[key] = blockValue;

                    continue;
                }

                i++;
            }

            return result;
        }
    }
}
