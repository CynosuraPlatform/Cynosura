﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Infrastructure.Authorization
{
    public interface IPolicyModule
    {
        void RegisterPolicies(AuthorizationOptions options);
    }
}
