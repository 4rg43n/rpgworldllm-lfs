using System;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class PlayerDefinition : CharacterDefinition
    {
        public string userNotes = ""; // notes from the player that get added to prompts

        public override void CopyFrom(BaseStoryObject other)
        {
            if (other is PlayerDefinition otherPlayer)
            {
                base.CopyFrom(otherPlayer);
                userNotes = otherPlayer.userNotes;
            }
            else 
                Debug.LogError("Cannot copy from " + other.GetType().Name);
        }
        
        public override BaseStoryObject Clone()
        {
            var clone = new PlayerDefinition();
            clone.CopyFrom(this);
            return clone;
        }
    }
}
