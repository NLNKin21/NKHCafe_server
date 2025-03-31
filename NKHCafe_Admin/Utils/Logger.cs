using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHCafe_Admin.Utils
{
    public static class Logger
    {
        private static string _logFilePath = "server_log.txt";
        private static readonly object _logLock = new object();

        public static void Initialize(string logPath = "server_log.txt")
        {
            _logFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logPath);
            Log($"--- Logger Initialized: {_logFilePath} ---");
        }

        public static void Log(string message)
        {
            try
            {
                string logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff} - {message}";
                Console.WriteLine(logEntry); // Ghi ra Console
                lock (_logLock) // Lock để tránh ghi file đồng thời
                {
                    System.IO.File.AppendAllText(_logFilePath, logEntry + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"!!! Logger Error: {ex.Message} !!!");
            }
        }
    }
}