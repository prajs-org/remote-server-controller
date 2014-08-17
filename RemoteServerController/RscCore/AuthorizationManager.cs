using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace RscCore
{
    class AuthorizationManager : ServiceAuthorizationManager
    {
        public override bool CheckAccess(OperationContext operationContext, ref System.ServiceModel.Channels.Message message)
        {
            return base.CheckAccess(operationContext, ref message);
        }
    }
}
