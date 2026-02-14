namespace RPGWorldLLM.Logging
{
    public class StoryLogData : LogMessageData
    {
        public override string LogFileName => "story.log";

        public StoryLogData(string messageType, string data) : base(messageType, data) { }
    }
}
