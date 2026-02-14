namespace RPGWorldLLM.Logging
{
    public abstract class LogMessageData
    {
        public string messageType;
        public string data;

        public abstract string LogFileName { get; }

        protected LogMessageData(string messageType, string data)
        {
            this.messageType = messageType;
            this.data = data;
        }
    }
}
