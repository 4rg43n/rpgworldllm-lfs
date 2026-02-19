using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    public class StoryManager : MonoBehaviour
    {
        private static StoryManager instance;
        public static StoryManager Instance => instance;

        public string test_story_load = "story_example.txt";

        public StoryDefinition story;
        
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
            List<BaseStoryObject> storyObjects = story.ProcessSections();

            Debug.Log("Story loaded");
            
            if (storyObjects.Count > 0)
            {
                foreach (BaseStoryObject obj in storyObjects)
                {
                    Debug.Log(obj.ToString());
                }
            }
        }
    }

}
