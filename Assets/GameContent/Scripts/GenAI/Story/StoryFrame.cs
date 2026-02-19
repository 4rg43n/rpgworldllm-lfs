using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class StoryFrame
    {
        public StoryDefinition currentStory;
        List<BaseStoryObject> storyObjects=new List<BaseStoryObject>();

        public string response = ""; // the LLM response
        public string playerResponse = "";

        public void AddResponse(string response)
        {
            this.response = response;
            currentStory.memoryItems.Add(new MemoryItem(response));
        }

        public static StoryFrame CreateFrom(StoryDefinition story, List<BaseStoryObject> storyObjects)
        {
            StoryFrame frame = new StoryFrame();
            frame.currentStory=story.Clone() as StoryDefinition;

            foreach (BaseStoryObject obj in storyObjects)
            {
                frame.storyObjects.Add(obj.Clone());
            }
            
            frame.response=story.parameters["first_message"];

            return frame;
        }
        
        public StoryFrame Clone()
        {
            StoryFrame newFrame = new StoryFrame();
            newFrame.currentStory = currentStory.Clone() as StoryDefinition;

            foreach (BaseStoryObject obj in storyObjects)
            {
                newFrame.storyObjects.Add(obj.Clone());
            }
            
            return newFrame;
        }
    }
}
