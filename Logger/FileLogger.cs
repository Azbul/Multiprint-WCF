using System;
using System.IO;
using System.Text;

namespace Logger
{
    public static class FileLogger
    {
        private static readonly string _logFileDir = $"AppLogs";

        private static readonly string _logFilePath = $"{_logFileDir}/{DateTime.Now.ToString("d")} logs.txt";

        private static object _lockObj = new object();

        static FileLogger()
        {
            //create dir if not exsits 
            Directory.CreateDirectory($@"{Environment.CurrentDirectory}\{_logFileDir}");
        }

        public static void Log(string msg)
        {
            lock (_lockObj)
            {
                using (StreamWriter writer = new StreamWriter(_logFilePath, true, Encoding.Default))
                {
                    writer.WriteLine($"{DateTime.Now} - {msg}");
                }
            }
        }
    }
}
