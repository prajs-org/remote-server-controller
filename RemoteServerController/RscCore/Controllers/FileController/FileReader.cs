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

    internal class FileReader
    {
        /// <summary>
        /// Instance of FileManager.
        /// Contains all importatnt information about file.
        /// </summary>
        FileManager fileManager;

        /// <summary>
        /// Create instance of FileReader for given FileManager.
        /// </summary>
        /// <param name="fileManager">Instance of FileManager</param>
        public FileReader(FileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        /// <summary>
        /// Read first [length] characters of file.
        /// </summary>
        /// <param name="length">How many characters will be read from the start of file</param>
        /// <param name="content">Read content of file</param>
        /// <returns>Action result</returns>
        public ReturnCodes.ActionReturnCode ReadStart(int length, out string content)
        {
            return _readInterval(0, length, out content);
        }

        /// <summary>
        /// Read last [length] characters of file.
        /// </summary>
        /// <param name="length">How many characters will be read from the end of file</param>
        /// <param name="content">Read content of file</param>
        /// <returns>Action result</returns>
        public ReturnCodes.ActionReturnCode ReadEnd(int length, out string content)
        {
            try
            {
                if (File.Exists(this.fileManager.FullPath))
                {
                    FileInfo fileInfo = new FileInfo(this.fileManager.FullPath);
                    return _readInterval(Convert.ToInt32(fileInfo.Length - length), length, out content);
                }
                else
                {
                    content = String.Empty;
                    RscLog.Alert("Could not read file<{0}> because it does not exist.", this.fileManager.FullPath);
                    return ReturnCodes.ActionReturnCode.FileNotExist;
                }
            }
            catch (Exception ex)
            {
                content = String.Empty;
                RscLog.Error(ex, "Error during reading of file<" + this.fileManager.FullPath + ">.");
                return ReturnCodes.ActionReturnCode.UnknownError;
            }
        }

        /// <summary>
        /// Read interval of characters from file.
        /// </summary>
        /// <param name="from">First character to read (including)</param>
        /// <param name="to">How many characters will be read</param>
        /// <param name="content">Read content of file</param>
        /// <returns>Action result</returns>
        public ReturnCodes.ActionReturnCode ReadInterval(int start, int length, out string content)
        {
            return _readInterval(start, length, out content);
        }

        /// <summary>
        /// Read interval of characters from file.
        /// </summary>
        /// <param name="from">First character to read (including)</param>
        /// <param name="to">How many characters will be read</param>
        /// <param name="content">Read content of file</param>
        /// <returns>Action result</returns>
        private ReturnCodes.ActionReturnCode _readInterval(int start, int length, out string content)
        {
            // Empty by default
            content = String.Empty;
            
            // Check if file exists
            if (File.Exists(this.fileManager.FullPath))
            {
                try
                {
                    // Get info about file
                    FileInfo fileInfo = new FileInfo(this.fileManager.FullPath);
                    int fileLength = Convert.ToInt32(fileInfo.Length);
                    // Check if arguments are correct
                    if (start < 0 || length < 0)
                    {
                        RscLog.Error("Error during reading of file<{0}>. Arguments have to be greater than zero: start<{1}>, length<{2}>", this.fileManager.FullPath, start, length);
                        return ReturnCodes.ActionReturnCode.FormatError;
                    }
                    // Check if start is in range
                    if (start >= fileLength)
                    {
                        RscLog.Error("Error during reading of file<{0}>. Start is behind size of file: start<{1}>, file size<{2}>", this.fileManager.FullPath, start, fileLength);
                        return ReturnCodes.ActionReturnCode.FormatError;
                    }
                    // Modify length if necessary (if length is behind the size of file)
                    length = start + length <= fileLength ? length : fileLength - start;
                    // Read the file
                    using (StreamReader sr = new StreamReader(this.fileManager.FullPath))
                    {
                        // Read file
                        char[] buffer = new char[length];
                        var i = sr.ReadBlock(buffer, start, length);
                        content = new string(buffer);
                        return ReturnCodes.ActionReturnCode.OK;
                    }
                }
                catch (Exception ex)
                {
                    RscLog.Error(ex, "Error during reading of file<" + this.fileManager.FullPath + ">.");
                    return ReturnCodes.ActionReturnCode.UnknownError;
                }
            }
            else
            {
                RscLog.Alert("Could not read file<{0}> because it does not exist.", this.fileManager.FullPath);
                return ReturnCodes.ActionReturnCode.FileNotExist;
            }            
        }
    }
}
