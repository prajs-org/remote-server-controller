using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Core;
using System.Reflection;

namespace RscLog
{
    public class Log4NetWrapper : ILog
    {
        #region Construction - singleton

        /// <summary>
        /// Instance of this wrapper class.
        /// </summary>
        static Log4NetWrapper wrapper = null;
        /// <summary>
        /// Instance of Log4Net logger.
        /// </summary>
        ILogger log4net;
        /// <summary>
        /// Private constructor. Use function Instance to get instance of this class.
        /// </summary>
        /// <param name="appName">Name of logger</param>
        private Log4NetWrapper(string appName)
        {
            //log4net = LoggerManager.GetLogger(Assembly.GetExecutingAssembly(), appName);
            log4net = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), MethodBase.GetCurrentMethod().DeclaringType);
        }
        /// <summary>
        /// Get instance of log4net wrapper.
        /// Implemented as singleton.
        /// If instance alredy exists, just return it. If instance does not exist yet, create it and then return it.
        /// </summary>
        /// <param name="appName">Name of logger</param>
        /// <returns>Instance of log4net wrapper.</returns>
        public static Log4NetWrapper Instance(string appName)
        {
            if (wrapper == null)
                wrapper = new Log4NetWrapper(appName);
            return wrapper;
        }

        #endregion

        #region Public functions

        public void Debug(string message, params object[] args)
        {
            logMessage(Level.Debug, message, args);
        }

        public void Info(string message, params object[] args)
        {
            logMessage(Level.Info, message, args);
        }

        public void Alert(string message, params object[] args)
        {
            logMessage(Level.Alert, message, args);
        }

        public void Error(string message, params object[] args)
        {
            logMessage(Level.Error, message, args);
        }

        public void Debug(Exception exception, string comment)
        {
            logException(Level.Debug, comment, exception);
        }

        public void Info(Exception exception, string comment)
        {
            logException(Level.Info, comment, exception);
        }

        public void Alert(Exception exception, string comment)
        {
            logException(Level.Alert, comment, exception);
        }

        public void Error(Exception exception, string comment)
        {
            logException(Level.Error, comment, exception);
        }

        private void logMessage(Level level, string message, params object[] args)
        {
            log4net.Log(typeof(Log4NetWrapper), level, String.Format(message, args), null);
        }

        private void logException(Level level, string message, Exception exception)
        {
            log4net.Log(typeof(Log4NetWrapper), level, message, exception);
        }

        #endregion
    }
}
