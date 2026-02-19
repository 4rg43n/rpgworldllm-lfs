using System;
using System.Collections.Generic;
using RPGWorld.Chat;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    public class StoryManager : MonoBehaviour
    {
        private static StoryManager instance;
        public static StoryManager Instance => instance;

        public string test_story_load = "story_example.txt";

        public StoryDefinition story;
        public List<BaseStoryObject> storyObjects=new List<BaseStoryObject>();
        
        public List<StoryFrame> storyFrames = new List<StoryFrame>();
        
        public PlayerDefinition player=new PlayerDefinition();
        public StoryFrame CurrentStoryFrame => storyFrames[storyFrames.Count-1];
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            story = StoryDefinition.LoadFromTextAssetST(test_story_load);
            storyObjects = story.ProcessSections();
            
            StoryFrame frame = StoryFrame.CreateFrom(story, storyObjects);
            storyFrames.Add(frame);

            CurrentStoryFrame.AddResponse(CurrentStoryFrame.currentStory.parameters["first_message"]);
            ChatHistory.Instance.AddText("first_message", CurrentStoryFrame.response);
            
            Debug.Log("Story loaded");
        }
    }

}
