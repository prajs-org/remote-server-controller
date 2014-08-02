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

namespace RscLog
{
    /// <summary>
    /// TODO: implement some real logger
    /// </summary>
    public static class Log
    {
        public enum LogLevel { Silent, Debug, Info, Warning, Error, Fatal }

        public static LogLevel Level { set; get; }

        public static void Debug(string message, params object[] args)
        {
            if (Level <= LogLevel.Debug)
                WriteLog(LogLevel.Debug, message, args);
        }

        public static void Info(string message, params object[] args)
        {
            if (Level <= LogLevel.Info)
                WriteLog(LogLevel.Info, message, args);
        }

        public static void Warning(string message, params object[] args)
        {
            if (Level <= LogLevel.Warning)
                WriteLog(LogLevel.Warning, message, args);
        }

        public static void Error(string message, params object[] args)
        {
            if (Level <= LogLevel.Error)
                WriteLog(LogLevel.Error, message, args);
        }

        public static void Fatal(string message, params object[] args)
        {
            if (Level <= LogLevel.Fatal)
                WriteLog(LogLevel.Fatal, message, args);
        }

        private static void WriteLog(LogLevel level, string message, params object[] args)
        {
            try
            {
                Console.WriteLine(new StringBuilder(level.ToString()).Append(": ")
                    .Append(String.Format(message, args)));
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
