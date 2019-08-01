using log4net;
using System;

namespace RSH.Utility
{
    public class LogHelper
    {
        private static readonly ILog Log4Net = LogManager.GetLogger("Logger");

        public LogHelper()
        {
            Log4Net.Info($"Logging started by {Environment.MachineName}");
        }

        public static void Debug(string message, Exception exception = null)
        {
            if (Log4Net.IsDebugEnabled)
                Log4Net.Debug(message, exception);
        }

        public static void Info(string message, Exception exception = null)
        {
            if (Log4Net.IsInfoEnabled)
                Log4Net.Info(message, exception);
        }

        public static void Warning(string message, Exception exception = null)
        {
            if (Log4Net.IsWarnEnabled)
                Log4Net.Warn(message, exception);
        }

        public static void Error(string message, Exception exception = null)
        {
            if (Log4Net.IsErrorEnabled)
                Log4Net.Error(message, exception);
        }

        public static void Fatal(string message, Exception exception = null)
        {
            if (Log4Net.IsFatalEnabled)
                Log4Net.Fatal(message, exception);
        }
    }
}