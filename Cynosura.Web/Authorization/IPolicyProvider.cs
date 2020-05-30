using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace Cynosura.Web.Authorization
{
    public interface IPolicyProvider
    {
        void RegisterPolicies(AuthorizationOptions options);
    }
}
