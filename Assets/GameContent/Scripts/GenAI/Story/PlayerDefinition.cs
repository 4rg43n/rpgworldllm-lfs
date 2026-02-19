using System;
using UnityEngine;

namespace RPGWorldLLM.GenAI.Story
{
    [Serializable]
    public class PlayerDefinition : CharacterDefinition
    {
        public string userNotes = ""; // notes from the player that get added to prompts
    }
}
