// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace Cynosura.IdentityServer;

internal sealed class IdentityResourceDefinition : ResourceDefinition
{
    public IdentityResourceDefinition()
    {
        Profile = "API";
    }
}
