using System;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class PlayerDefinition : CharacterDefinition
    {
        public override BaseStoryObject Clone()
        {
            var clone = new PlayerDefinition();
            clone.CopyFrom(this);
            return clone;
        }
    }
}
