using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RscLog
{
    internal interface IRscLog
    {
        // Standard logs
        void Debug(string message, params object[] args);
        void Info(string message, params object[] args);
        void Alert(string message, params object[] args);
        void Error(string message, params object[] args);
        // Audit logs
        void AuditIncoming(string message, params object[] args);
        void AuditSuccess(string message, params object[] args);
        void AuditFailed(string message, params object[] args);
        // Exception logs
        void Debug(Exception exception, string comment);
        void Info(Exception exception, string comment);
        void Alert(Exception exception, string comment);
        void Error(Exception exception, string comment);
    }
}
