using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FreeAdvice.Common
{
    public interface ILogger
    {
        void LogError(string error);
        void LogInfo(string info);
        void Debug(string debug);
    }

    public class Logger : ILogger
    {
        public EventHandler<BoringEventArgs> LoggingEvents;

        public void ThrowEvent(string logText)
        {
            if(LoggingEvents != null)
                LoggingEvents(this, new BoringEventArgs() { Arguments = logText});
        }

        public void LogError(string error)
        {
            ThrowEvent("ERROR: " + error);
        }

        public void LogInfo(string info)
        {
            ThrowEvent("INFO: " + info);
        }

        public void Debug(string debug)
        {
            ThrowEvent("DEBUG: " + debug);
        }
    }

    public class BoringEventArgs : EventArgs
    {
        public string Arguments { get; set;}
    }
}
