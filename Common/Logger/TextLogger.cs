using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logger
{
    public class TextLogger : ILogger
    {
        private string filePath;
        private bool automaticExport;
        private StringBuilder builder;
        private string format = "[{0}] - ({1}) : {2}";

        public TextLogger(string filePath, bool automaticExport)
        {
            this.filePath = filePath;
            this.automaticExport = automaticExport;
            builder = new StringBuilder();
            if (!Directory.Exists(filePath))
                File.Create(filePath);
            else
                this.filePath = GetFileName();

        }

        public void Debug(string message)
        {
            builder.AppendLine(String.Format(format, LogLevel.DEBUG, DateTime.Now.ToString(), message));
            if (automaticExport)
                File.WriteAllText(filePath, builder.ToString());
        }

        public void Error(string message)
        {
            builder.AppendLine(String.Format(format, LogLevel.ERROR, DateTime.Now.ToString(), message));
            if (automaticExport)
                File.WriteAllText(filePath, builder.ToString());
        }

        public void ExportLogs()
        {
            File.WriteAllText(filePath, builder.ToString());
        }

        public void Fatal(string message)
        {
            builder.AppendLine(String.Format(format, LogLevel.FATAL, DateTime.Now.ToString(), message));
            if (automaticExport)
                File.WriteAllText(filePath, builder.ToString());
        }

        public List<string> GetLogs()
        {
            return builder.ToString().Split('\n').ToList();
        }

        public void Info(string message)
        {
            builder.AppendLine(String.Format(format, LogLevel.INFO, DateTime.Now.ToString(), message));
            if (automaticExport)
                File.WriteAllText(filePath, builder.ToString());
        }

        public void Warning(string message)
        {
            builder.AppendLine(String.Format(format, LogLevel.WARNING, DateTime.Now.ToString(), message));
            if (automaticExport)
                File.WriteAllText(filePath, builder.ToString());
        }

        private string GetFileName()
        {
            return filePath + $"{DateTime.Now.Day}_{DateTime.Now.Month}_{DateTime.Now.Year}T{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}.txt";
        }
    }
}
