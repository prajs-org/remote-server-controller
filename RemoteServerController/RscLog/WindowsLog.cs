/******************************************************************************
 * Remote Server Controller, http://rsc.codeplex.com                          *
 *                                                                            *
 * Copyright (C) 2014 Karel Prajs                                             *
 *                                                                            *
 * This program is free software: you can redistribute it and/or modify       *
 * it under the terms of the GNU General Public License as published by       *
 * the Free Software Foundation, either version 3 of the License, or          *
 * (at your option) any later version.                                        *
 *                                                                            *
 * This program is distributed in the hope that it will be useful,            *
 * but WITHOUT ANY WARRANTY; without even the implied warranty of             *
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the              *
 * GNU General Public License for more details.                               *
 *                                                                            *
 * You should have received a copy of the GNU General Public License          *
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.      *
 ******************************************************************************/
namespace RscLog
{
    // System namespaces
    using System;
    using System.Diagnostics;

    // Project namespaces
    using RscConfig;

    /// <summary>
    /// Default Windows logger.
    /// </summary>
    public class WindowsLog : IRscLog
    {
        #region Construction - singleton

        static WindowsLog instance = null;

        static EventLog eventLog = new EventLog();

        private WindowsLog(string appName)
        {
            eventLog.Source = appName;
        }

        public static WindowsLog Instance(string appName)
        {
            if (instance == null)
                instance = new WindowsLog(appName);
            return instance;
        }

        #endregion

        #region Public functions

        public void Debug(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Information, message, args);
        }

        public void Debug(Exception exception, string comment)
        {
            WriteException(EventLogEntryType.Information, exception, comment);
        }

        public void Info(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Information, message, args);
        }

        public void Info(Exception exception, string comment)
        {
            WriteException(EventLogEntryType.Information, exception, comment);
        }

        public void Alert(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Warning, message, args);
        }

        public void Alert(Exception exception, string comment)
        {
            WriteException(EventLogEntryType.Warning, exception, comment);
        }

        public void Error(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Error, message, args);
        }

        public void Error(Exception exception, string comment)
        {
            WriteException(EventLogEntryType.Error, exception, comment);
        }

        public void AuditIncoming(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Information, message, args);
        }

        public void AuditSuccess(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Information, message, args);
        }

        public void AuditFailed(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Warning, message, args);
        }

        #endregion

        #region Private functions

        private void WriteException(EventLogEntryType level, Exception exception, string comment)
        {
            if (exception != null)
            {
                WriteLog(level, "Exception ({0}): {1} --- InnerExcetion: {2}.",
                    comment,
                    exception.Message,
                    exception.InnerException != null ? exception.InnerException.Message : String.Empty);
            }
            else
            {
                WriteLog(level, "Empty exception ({0})", comment ?? String.Empty);
            }
        }

        private void WriteLog(EventLogEntryType level, string message, params object[] args)
        {
            try
            {
                eventLog.WriteEntry(String.Format(message, args), level);
            }
            catch
            {
                eventLog.WriteEntry(String.Format("INVALID LOG RECORD: {0}", message ?? String.Empty), EventLogEntryType.Error);
            }
        }

        #endregion
    }
}
