// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Cynosura.IdentityServer;
using Cynosura.IdentityServer.Authentication;
using Cynosura.IdentityServer.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authentication;

/// <summary>
/// Extension methods to configure authentication for existing APIs coexisting with an Authorization Server.
/// </summary>
public static class AuthenticationBuilderExtensions
{
    private const string IdentityServerJwtNameSuffix = "API";

    private static readonly PathString DefaultIdentityUIPathPrefix =
        new PathString("/Identity");

    /// <summary>
    /// Adds an authentication handler for an API that coexists with an Authorization Server.
    /// </summary>
    /// <param name="builder">The <see cref="AuthenticationBuilder"/>.</param>
    /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
    public static AuthenticationBuilder AddIdentityServerJwt(this AuthenticationBuilder builder)
    {
        var services = builder.Services;
        services.TryAddSingleton<IIdentityServerJwtDescriptor, IdentityServerJwtDescriptor>();
        services.TryAddEnumerable(ServiceDescriptor
            .Transient<IConfigureOptions<JwtBearerOptions>, IdentityServerJwtBearerOptionsConfiguration>(JwtBearerOptionsFactory));

        services.AddAuthentication(IdentityServerJwtConstants.IdentityServerJwtScheme)
            .AddPolicyScheme(IdentityServerJwtConstants.IdentityServerJwtScheme, null, options =>
            {
                options.ForwardDefaultSelector = new IdentityServerJwtPolicySchemeForwardSelector(
                    DefaultIdentityUIPathPrefix,
                    IdentityServerJwtConstants.IdentityServerJwtBearerScheme).SelectScheme;
            })
            .AddJwtBearer(IdentityServerJwtConstants.IdentityServerJwtBearerScheme, null, o => { });

        return builder;

        static IdentityServerJwtBearerOptionsConfiguration JwtBearerOptionsFactory(IServiceProvider sp)
        {
            var schemeName = IdentityServerJwtConstants.IdentityServerJwtBearerScheme;

            var localApiDescriptor = sp.GetRequiredService<IIdentityServerJwtDescriptor>();
            var hostingEnvironment = sp.GetRequiredService<IWebHostEnvironment>();
            var apiName = hostingEnvironment.ApplicationName + IdentityServerJwtNameSuffix;

            return new IdentityServerJwtBearerOptionsConfiguration(schemeName, apiName, localApiDescriptor);
        }
    }
}
