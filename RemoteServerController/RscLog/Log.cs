/*
Copyright (C) 2014 Karel Prajs
This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace RscLog
{
    /// <summary>
    /// TODO: implement some real logger
    /// </summary>
    public static class Log
    {
        static EventLog eventLog = new EventLog();

        static Log()
        {
            // Default init if none set better
            Init("RemoteServerController");
        }

        public static void Init(string appName)
        {
            eventLog.Source = appName;
        }

        public static void Info(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Information, message, args);
        }

        public static void Info(Exception exception, string comment)
        {
            WriteException(EventLogEntryType.Information, exception, comment);
        }

        public static void Warning(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Warning, message, args);
        }

        public static void Warning(Exception exception, string comment)
        {
            WriteException(EventLogEntryType.Warning, exception, comment);
        }

        public static void Error(string message, params object[] args)
        {
            WriteLog(EventLogEntryType.Error, message, args);
        }

        public static void Error(Exception exception, string comment)
        {
            WriteException(EventLogEntryType.Error, exception, comment);
        }

        private static void WriteException(EventLogEntryType level, Exception exception, string comment)
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

        private static void WriteLog(EventLogEntryType level, string message, params object[] args)
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
    }
}
