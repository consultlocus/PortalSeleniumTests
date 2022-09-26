using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsultlocusSelenium.Helpers
{
    public static class Logger
    {
        public static string LogsDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Helpers", "Logs");

        public static void CreateLog(string name)
        {
            var path = Path.Combine(LogsDirectory, name);
            var file = File.Create(path);
            file.Close();
        }

        public static void WriteLinesToLog(string name, string lines)
        {
            var path = Path.Combine(LogsDirectory, name);
            File.AppendAllText(path, lines + Environment.NewLine);
        }

        public static bool IsLogFileEmpty(string name)
        {
            var path = Path.Combine(LogsDirectory, name);
            var text = File.ReadAllText(path);
            if (text.Length == 0) return true;

            return false;
        }

        public static string GetLinesFromLogFile(string name)
        {
            var path = Path.Combine(LogsDirectory, name);
            var text = File.ReadAllText(path);

            return text;
        }
    }
}