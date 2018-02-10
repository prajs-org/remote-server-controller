/******************************************************************************
 * Remote Server Controller                                                   *
 * https://github.com/prajs-org/remote-server-controller                      *
 * Copyright (C) 2014-2018 Karel Prajs, karel@prajs.org                        *
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Core;
using System.Reflection;

namespace RscLog
{
    public class Log4NetWrapper : IRscLog
    {
        #region Construction - singleton

        /// <summary>
        /// Instance of this wrapper class.
        /// </summary>
        static Log4NetWrapper wrapper = null;
        /// <summary>
        /// Instance of basic logger.
        /// </summary>
        ILog logAppender;
        /// <summary>
        /// Instance of audit logger.
        /// </summary>
        ILog auditAppender;
        /// <summary>
        /// Private constructor. Use function Instance to get instance of this class.
        /// </summary>
        /// <param name="appName">Name of logger</param>
        private Log4NetWrapper(string appName)
        {
            logAppender = LogManager.GetLogger("LogAppender");
            auditAppender = LogManager.GetLogger("AuditAppender");
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
            logAppender.Logger.Log(typeof(Log4NetWrapper), level, String.Format(message, args), null);
        }

        private void logException(Level level, string message, Exception exception)
        {
            logAppender.Logger.Log(typeof(Log4NetWrapper), level, message, exception);
        }

        public void AuditIncoming(string apiKey, string message, params object[] args)
        {
            auditAppender.Logger.Log(typeof(Log4NetWrapper), Level.Info, BuildAuditMessage(apiKey, message, args), null);
        }

        public void AuditSuccess(string apiKey, string message, params object[] args)
        {
            auditAppender.Logger.Log(typeof(Log4NetWrapper), Level.Info, BuildAuditMessage(apiKey, message, args), null);
        }

        public void AuditFailed(string apiKey, string message, params object[] args)
        {
            auditAppender.Logger.Log(typeof(Log4NetWrapper), Level.Warn, BuildAuditMessage(apiKey, message, args), null);
        }

        #endregion

        #region Private

        private string BuildAuditMessage(string apiKey, string message, params object[] args)
        {
            StringBuilder sb = new StringBuilder("[")
                .Append(apiKey)
                .Append("] ")
                .Append(String.Format(message, args));
            return sb.ToString();
        }

        #endregion
    }
}
