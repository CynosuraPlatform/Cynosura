﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Web.Authorization
{
    public interface IPolicyProvider
    {
        void RegisterPolicies(AuthorizationOptions options);
    }
}