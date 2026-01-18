using System;

namespace CrossQuestUI.Services
{
    public interface IStreamLogger
    {
        public event EventHandler OnMessage;
        public void WriteMessage(string message);
    }
}