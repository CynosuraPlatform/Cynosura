// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Linq;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cynosura.IdentityServer;

internal sealed class DefaultClientRequestParametersProvider : IClientRequestParametersProvider
{
    public DefaultClientRequestParametersProvider(
        IAbsoluteUrlFactory urlFactory,
        IOptions<ApiAuthorizationOptions> options)
    {
        UrlFactory = urlFactory;
        Options = options;
    }

    public IAbsoluteUrlFactory UrlFactory { get; }

    public IOptions<ApiAuthorizationOptions> Options { get; }

    public IDictionary<string, string> GetClientParameters(HttpContext context, string clientId)
    {
        var client = Options.Value.Clients[clientId];
        var issuerNameService = context.RequestServices.GetRequiredService<IIssuerNameService>();
        var authority = issuerNameService.GetCurrentAsync().GetAwaiter().GetResult();
        if (!client.Properties.TryGetValue(ApplicationProfilesPropertyNames.Profile, out var type))
        {
            throw new InvalidOperationException($"Can't determine the type for the client '{clientId}'");
        }

        string responseType;
        switch (type)
        {
            case ApplicationProfiles.IdentityServerSPA:
            case ApplicationProfiles.SPA:
            case ApplicationProfiles.NativeApp:
                responseType = "code";
                break;
            default:
                throw new InvalidOperationException($"Invalid application type '{type}' for '{clientId}'.");
        }

        return new Dictionary<string, string>
        {
            ["authority"] = authority,
            ["client_id"] = client.ClientId,
            ["redirect_uri"] = UrlFactory.GetAbsoluteUrl(context, client.RedirectUris.First()),
            ["post_logout_redirect_uri"] = UrlFactory.GetAbsoluteUrl(context, client.PostLogoutRedirectUris.First()),
            ["response_type"] = responseType,
            ["scope"] = string.Join(" ", client.AllowedScopes)
        };
    }
}
