using System;
using System.Collections.Generic;
using System.Text;
using RPGWorldLLM.GenAI.Utils;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public abstract class BaseStoryObject
    {
        public const string KEY_TITLE_NAME="title";
        public const string KEY_TAG = "tag"; // short name, nickname, etc.
        public const string KEY_DESC="desc"; // short description, description, etc.
        public const string KEY_USER_NOTES="user_notes";
        public const string KEY_FIRST_MESSAGE="first_message";

        public const string TAG_NARRATOR = "narrator";
        public const string TAG_NAME = "character";
        public const string TAG_PHYSICAL_DESCRIPTION = "physical description";
        public const string TAG_USER_NOTES = "user_notes";
        public const string TAG_FIRST_MESSAGE = "first_message";
        
        public int id = System.Guid.NewGuid().GetHashCode(); // unique id for the object
        public Dictionary<string, string> objectParameters = new Dictionary<string, string>();

        public string Parameter(string key)
        {
            if (objectParameters.ContainsKey(key))
            {
                return objectParameters[key];
            }
            else
            {
                Debug.LogError("Tried to get parameter that doesn't exist: " + key + "");
                return "";
            }
        }

        protected Dictionary<string, string> CreateDeepCopyOfParameters()
        {
            Dictionary<string, string> newDict = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> kvp in objectParameters)
            {
                newDict.Add(kvp.Key, kvp.Value);
            }
            return newDict;
        }

        public virtual void CopyFrom(BaseStoryObject other)
        {
            objectParameters=CreateDeepCopyOfParameters();
        }
        
        public abstract BaseStoryObject Clone(); // this method should return a deep copy of the object

        override public string ToString()
        {
            return Parameter(KEY_TITLE_NAME) + " (" + id + ")" + " - " +
                   Parameter(KEY_DESC) + " - " +
                   Parameter(KEY_TAG);
        }
    }
}


