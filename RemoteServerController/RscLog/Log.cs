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

        public enum LogLevel { Silent, Debug, Info, Warning, Error, Fatal }

        public static LogLevel Level
        { 
            set;
            get;
        }

        public static void Init(string appName)
        {
            eventLog.Source = appName;
        }

        public static void Info(string message, params object[] args)
        {
            if (Level <= LogLevel.Info)
                WriteLog(EventLogEntryType.Information, message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            if (Level <= LogLevel.Warning)
                WriteLog(EventLogEntryType.Warning, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            if (Level <= LogLevel.Error)
                WriteLog(EventLogEntryType.Error, message, args);
        }

        private static void WriteLog(EventLogEntryType level, string message, params object[] args)
        {
            try
            {
                eventLog.WriteEntry(message, level);
            }
            catch (Exception)
            {
                Console.Write(level.ToString() + ": (format exception) " + message + " - ARGUMENTS:");
                foreach(var item in args)
                {
                    Console.Write(" <" + item + ">");
                }
                Console.WriteLine();
            }
        }
    }
}
