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
namespace RscCore.Controllers.FileController
{
    // System namespaces
    using System;
    using System.Linq;
    using System.ServiceProcess;
    using System.IO;

    // Project namespaces
    using RscLog;
    using RscConfig;
    using RscInterface;

    /// <summary>
    /// Controller class designated for controlling files on remote server.
    /// Each object of class FileManager can handle single file.
    /// </summary>
    public sealed class FileManager
    {
        #region Construction

        /// <summary>
        /// Create instance of FileManager class. Instance can be created only by ControlFactory. If you create it directly, all permissions will be set to false.
        /// </summary>
        /// <param name="serviceName">Name of service.</param>
        public FileManager(string fileAlias)
        {
            this.FileAlias = fileAlias;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Full path to file
        /// </summary>
        public string FileAlias
        {
            internal set;
            get;
        }
        /// <summary>
        /// Full path to file
        /// </summary>
        public string FullPath
        {
            internal set;
            get;
        }

        private string content;
        /// <summary>
        /// Content of file
        /// </summary>
        public string Content
        {
            get { return this.content; }
        }

        #endregion

        #region Security properties

        /// <summary>
        /// Flag if service is allowed to be read. Can be set by ControlFactory only.
        /// </summary>
        private bool allowRead = false;
        /// <summary>
        /// Flag if service can be started.
        /// </summary>
        public bool AllowRead
        {
            get { return this.allowRead; }
        }

        #endregion

        #region Public functions

        public FileReadResult Read()
        {
            // Check permission first
            if (this.AllowRead)
            {
                var reader = new FileReader(this);
                var result = reader.ReadStart(Int32.MaxValue, out this.content);
                return new FileReadResult(this.FullPath, result, this.content);
            }
            else
            {
                RscLog.AuditFailed("File<{0}> cannot be read because it is not allowed to read.", this.FileAlias);
                return new FileReadResult(this.FullPath, ReturnCodes.ActionReturnCode.NotAllowed, String.Empty);
            }
        }

        public FileReadResult ReadStart(int length)
        {
            // Check permission first
            if (this.AllowRead)
            {
                var reader = new FileReader(this);
                var result = reader.ReadStart(length, out this.content);
                return new FileReadResult(this.FullPath, result, this.content);
            }
            else
            {
                RscLog.AuditFailed("File<{0}> cannot be read because it is not allowed to read.", this.FileAlias);
                return new FileReadResult(this.FullPath, ReturnCodes.ActionReturnCode.NotAllowed, String.Empty);
            }
        }

        public FileReadResult ReadEnd(int length)
        {
            // Check permission first
            if (this.AllowRead)
            {
                var reader = new FileReader(this);
                var result = reader.ReadEnd(length, out this.content);
                return new FileReadResult(this.FullPath, result, this.content);
            }
            else
            {
                RscLog.AuditFailed("File<{0}> cannot be read because it is not allowed to read.", this.FileAlias);
                return new FileReadResult(this.FullPath, ReturnCodes.ActionReturnCode.NotAllowed, String.Empty);
            }
        }

        public FileReadResult ReadInterval(int start, int length)
        {
            // Check permission first
            if (this.AllowRead)
            {
                var reader = new FileReader(this);
                var result = reader.ReadInterval(start, length, out this.content);
                return new FileReadResult(this.FullPath, result, this.content);
            }
            else
            {
                RscLog.AuditFailed("File<{0}> cannot be read because it is not allowed to read.", this.FileAlias);
                return new FileReadResult(this.FullPath, ReturnCodes.ActionReturnCode.NotAllowed, String.Empty);
            }
        }

        #endregion
    }
}
