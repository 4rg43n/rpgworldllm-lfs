namespace RPGWorldLLM.Logging
{
    public class PromptLogData : LogMessageData
    {
        public override string LogFileName => "prompt.log";

        public PromptLogData(string messageType, string data) : base(messageType, data) { }
    }
}
