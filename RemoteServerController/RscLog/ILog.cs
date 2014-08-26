using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RscLog
{
    internal interface ILog
    {
        // Standard logs
        void Debug(string message, params object[] args);
        void Info(string message, params object[] args);
        void Alert(string message, params object[] args);
        void Error(string message, params object[] args);
        // Exception logs
        void Debug(Exception exception, string comment);
        void Info(Exception exception, string comment);
        void Alert(Exception exception, string comment);
        void Error(Exception exception, string comment);
    }
}
