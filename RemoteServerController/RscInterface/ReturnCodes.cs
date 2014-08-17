using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RscInterface
{
    public static class ReturnCodes
    {
        /// <summary>
        /// Return codes for various actions
        /// </summary>
        public enum ActionReturnCode
        {
            OK,                     // OK (success)
            NotAllowed,             // Action is not allowed (not enough permissions)
            NotSupported,           // Action is not supported
            Timeout,                // Action failed on timeout
            UnmetRequirements,      // Requirements for action are not fulfilled
            UnknownError            // Some unspecified error
        }
    }
}
