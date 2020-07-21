using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Logger
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
        void Fatal(string message);
        void Error(string message);
        void Warning(string message);
        void ExportLogs();
        List<string> GetLogs();
    }
}
