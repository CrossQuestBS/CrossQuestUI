using System;

namespace CrossQuestUI.Services
{
    
    
    public class StreamLogger : IStreamLogger
    {
        public class StreamLoggerEventArgs : EventArgs
        {
            public string Message { get; set; }
        }

        public event EventHandler? OnMessage;

        public void WriteMessage(string message)
        {
            var eventArg = new StreamLoggerEventArgs();
            eventArg.Message = message;
            OnMessage.Invoke(this, eventArg);
        }
    }
}