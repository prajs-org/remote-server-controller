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

namespace RscConfig
{
    public static class Constants
    {
        public static int GeneralTimeout = 5000;

        public static string AppShortcut = "RSC";

        public static string AppName = "RemoteServerController";

        public static string AppDisplayName = "Remote Server Controller";

        public static string UknownToken = "Unknown";

        public enum ErrorCode { 
            OK,                     // OK (success)
            NotAllowed,             // Action is not allowed (not enough permissions)
            NotSupported,           // Action is not supported
            Timeout,                // Action failed on timeout
            UnmetRequirements,      // Requirements for action are not fulfilled
            UnknownError            // Some unspecified error
        }

        public enum ServiceActions {
            Start,
            Stop,
            StatusCheck
        }
    }
}
