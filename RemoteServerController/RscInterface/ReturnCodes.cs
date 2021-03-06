﻿/******************************************************************************
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

namespace RscInterface
{
    // System namespaces
    // -- none

    // Project namespaces
    // -- none 

    /// <summary>
    /// Return codes used within entire application.
    /// </summary>
    public static class ReturnCodes
    {
        /// <summary>
        /// Return codes for various actions.
        /// </summary>
        public enum ActionReturnCode
        {
            OK,                     // OK (success)
            NotAllowed,             // Action is not allowed (not enough permissions)
            NotSupported,           // Action is not supported
            Timeout,                // Action failed on timeout
            UnmetRequirements,      // Requirements for action are not fulfilled
            UnknownError,           // Some unspecified error
            FormatError,            // Request is in wrong format
        }
    }
}
