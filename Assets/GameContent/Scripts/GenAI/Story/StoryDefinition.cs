using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RPGWorldLLM.Logging;
using RPGWorldLLM.Utils;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class StoryDefinition : HistoryStoryObject
    {
        public override BaseStoryObject Clone()
        {
            StoryDefinition newStory = new StoryDefinition();
            newStory.CopyFrom(this);
            return newStory;
        }
    }

}
