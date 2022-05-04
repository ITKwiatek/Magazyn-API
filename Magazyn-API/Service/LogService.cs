using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Magazyn_API.Service
{
    public static class LogService
    {
        private const string logPath = @"C:\Temp\Wydania-Log\";
        private const string logFIleName = @"Log.txt";
        public static void SaveLog(string log)
        {
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            string time = DateTime.Now.ToString();

            File.AppendAllLines(logPath + logFIleName, new[] { time, log, "\n" });
        }
    }
}
