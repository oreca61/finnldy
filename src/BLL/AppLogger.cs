using System;
using System.IO;

namespace Finnldy.BLL
{
    public static class AppLogger
    {
        private static readonly string LogFilePath = "finnldy.log";

        public static void Info(string message)
        {
            Write("INFO", message);
        }

        public static void Error(string message)
        {
            Write("ERROR", message);
        }

        private static void Write(string level, string message)
        {
            string logLine = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                             + " [" + level + "] "
                             + message;

            File.AppendAllText(LogFilePath, logLine + Environment.NewLine);
        }
    }
}