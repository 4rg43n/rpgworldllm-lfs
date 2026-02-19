using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public abstract class BaseStoryObject
    {
        public int id = System.Guid.NewGuid().GetHashCode(); // unique id for the object
        public string name=""; // name of the story object
        public string tag = ""; // tag of the story object
        public string description=""; // description of the story object

        public virtual string PrintName
        {
            get => name;
        }
        public virtual string PrintDescription
        {
            get => description;
        }

        public abstract BaseStoryObject Clone(); // this method should return a deep copy of the object

        override public string ToString()
        {
            return name+" ("+id+")"+" - "+ PrintName +" - "+tag+" - "+PrintDescription.Substring(0,Math.Min(PrintDescription.Length, 10));
        }
    }
}


