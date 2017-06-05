/******************************************************************************
 * Remote Server Controller                                                   *
 * https://github.com/prajs-org/remote-server-controller                      *
 * Copyright (C) 2014-2017 Karel Prajs, karel@prajs.org                        *
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

namespace RscCore.Factories
{
    // System namespaces
    using System;
    using System.Reflection;
    using System.Collections.Generic;

    // Projet namespaces
    using RscCore.Controllers.FileController;
    using RscConfig;
    using RscLog;
    using RscCore.Security;

    internal static class FileManagerFactory
    {
        /// <summary>
        /// All paths loaded from config file.
        /// Key is alias, value is full path.
        /// </summary>
        static Dictionary<string, string> fullPaths = null;
        /// <summary>
        /// Initialize this class.
        /// </summary>
        static FileManagerFactory()
        {
                fullPaths = new Dictionary<string, string>();
                try
                {
                    foreach (var item in Configurator.Settings.Files.AllowedFiles)
                    {
                        AddFile file = (AddFile)item;
                        if (fullPaths.ContainsKey(file.Alias))
                        {
                            RscLog.Alert("Another definition of alias<{0}> found! I will use fullPath<{1}> from first occurence of this alias.",
                                file.Alias,
                                fullPaths[file.Alias]);
                        }
                        else
                        {
                            fullPaths.Add(file.Alias, file.FullPath);
                        }
                    }
                }
                catch (Exception ex)
                {
                    RscLog.Error(ex, "Could not load allowed files from config file. No file request will be processed.");
                    fullPaths.Clear();
                }
        }

        /// <summary>
        /// Create instance of file with configured permissions. The permissions are read-only in final object.
        /// Object may be created only if it is configured as allowed file in configuration file.
        /// </summary>
        /// <param name="serviceName">Full path to file</param>
        /// <returns>Instance of FileManager or null if file is not specified in configuration file.</returns>
        public static FileManager CreateFileManager(string fileAlias, string apiKey)
        {
            // This object will be returned if all succeeded
            FileManager fileManager = null;
            // Configuration of file
            AddFile fileConfiguration = null;
            // If this variable is set to TRUE, no permissions will be set.
            bool forbidAll = false;

            // ---------------------------------------------------------
            // SECURITY NOTICE:
            // All mandatory checks (like API key, IP address...) should be done within following
            // try-catch block. If anything fail, the forbidAll flag has to be set to TRUE.

            try
            {
                // Prepare object with no permissions
                fileManager = new FileManager(fileAlias);

                // Load configuration of file from configuration file
                if (false == Configurator.Settings.Files.AllowedFiles.GetFile(fileAlias, out fileConfiguration))
                {
                    // File is not configured and cannot be processed
                    RscLog.AuditFailed("Processing of file<{0}> is not allowed because it is not configured!", fileAlias);
                    forbidAll = true;
                }
                // Check if path is configured for this alias (this should not happen if we have the configuration above)
                else if (false == fullPaths.ContainsKey(fileAlias))
                {
                    // File is not configured and cannot be processed
                    RscLog.AuditFailed("Processing of file<{0}> is not allowed. Alias is configured but no path found!", fileAlias);
                    forbidAll = true;
                }
                // Check API Key -- TODO: refactor this as separate function (reusable)
                else if (Configurator.Settings.Security.CheckAPIKey)
                {
                    if (false == APIKeyManager.Instance().IsValidAPIKey(apiKey))
                    {
                        // Given API Key is not valid, request cannot be processed
                        RscLog.AuditFailed("Processing of file<{0}> is not allowed because of invalid APIKey<{1}>!", fileAlias, apiKey);
                        forbidAll = true;
                    }
                }
            }
            catch (Exception ex)
            {
                forbidAll = true;
                var message = "Permissions could not been processed correctly. Forbidding all permissions.";
                RscLog.Alert(ex, message);
                RscLog.AuditFailed(apiKey, message);
            }

            // ---------------------------------------------------------
            // SECURITY NOTICE:
            // The forbidAll flag is set now, so if it is still FALSE, you can start to allow some permissions.
            // Put your own checks inside the following if block.

            if (fileManager != null && fileConfiguration != null && forbidAll == false)
            {

                /*** HERE YOU CAN START TO ALLOWING COMMON PERMISSIONS ***/

                // --- Configure full path to file
                fileManager.GetType().GetField("fullPath", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(fileManager, fullPaths[fileAlias]);

                // --- Allow Start (service may be started)
                fileManager.GetType().GetField("allowRead", BindingFlags.NonPublic | BindingFlags.Instance)
                    .SetValue(fileManager, fileConfiguration.AllowRead);
            }

            return fileManager;
        }
    }
}