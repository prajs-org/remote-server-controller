using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RscConfig
{
    public static class Constants
    {
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
