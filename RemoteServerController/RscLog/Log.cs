﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace RscLog
{
    public static class Log
    {
        /// <summary>
        /// Logger type.
        /// </summary>
        public enum Logger { Windows, Log4Net }
        /// <summary>
        /// Instance of logger.
        /// </summary>
        private static ILog logger;
        /// <summary>
        /// Init logger with given log system.
        /// </summary>
        /// <param name="logger">Log system.</param>
        public static void Init(Logger logSystem, string appName)
        {
            switch (logSystem)
            {
                case Logger.Log4Net:
                    logger = Log4NetWrapper.Instance(appName);
                    break;
                case Logger.Windows:
                    logger = WindowsLog.Instance(appName);
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Default constructor if Init not executed.
        /// </summary>
        static Log()
        {
            Init(Logger.Windows, RscConfig.Constants.AppName);
        }

        public static void Debug(string message, params object[] args)
        {
            logger.Debug(message, args);
        }

        public static void Info(string message, params object[] args)
        {
            logger.Info(message, args);
        }

        public static void Alert(string message, params object[] args)
        {
            logger.Alert(message, args);
        }

        public static void Error(string message, params object[] args)
        {
            logger.Error(message, args);
        }

        public static void Debug(Exception exception, string comment)
        {
            logger.Debug(exception, comment);
        }

        public static void Info(Exception exception, string comment)
        {
            logger.Info(exception, comment);
        }

        public static void Alert(Exception exception, string comment)
        {
            logger.Alert(exception, comment);
        }

        public static void Error(Exception exception, string comment)
        {
            logger.Error(exception, comment);
        }
    }
}
