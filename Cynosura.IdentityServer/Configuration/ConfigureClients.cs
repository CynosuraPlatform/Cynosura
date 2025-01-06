// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Duende.IdentityServer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace Cynosura.IdentityServer;

internal sealed class ConfigureClients : IConfigureOptions<ApiAuthorizationOptions>
{
    private const string DefaultLocalSPARelativeRedirectUri = "/authentication/login-callback";
    private const string DefaultLocalSPARelativePostLogoutRedirectUri = "/authentication/logout-callback";

    private readonly IConfiguration _configuration;
    private readonly ILogger<ConfigureClients> _logger;

    public ConfigureClients(
        IConfiguration configuration,
        ILogger<ConfigureClients> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public void Configure(ApiAuthorizationOptions options)
    {
        foreach (var client in GetClients())
        {
            options.Clients.Add(client);
        }
    }

    internal IEnumerable<Client> GetClients()
    {
        var data = _configuration.Get<Dictionary<string, ClientDefinition>>();
        if (data != null)
        {
            foreach (var kvp in data)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation(LoggerEventIds.ConfiguringClient, "Configuring client '{ClientName}'.", kvp.Key);
                }
                var name = kvp.Key;
                var definition = kvp.Value;

                switch (definition.Profile)
                {
                    case ApplicationProfiles.SPA:
                        yield return GetSPA(name, definition);
                        break;
                    case ApplicationProfiles.IdentityServerSPA:
                        yield return GetLocalSPA(name, definition);
                        break;
                    case ApplicationProfiles.NativeApp:
                        yield return GetNativeApp(name);
                        break;
                    default:
                        throw new InvalidOperationException($"Type '{definition.Profile}' is not supported.");
                }
            }
        }
    }

    private static string FindWildcardReplacement(params string[] uris)
    {
        string replacement;
        for (int i = 0; ; i++)
        {
            replacement = "w" + i;
            if (uris.All(u => !u.Contains(replacement)))
            {
                break;
            }
        }
        return replacement;
    }

    private static string ReplaceWildcard(string uri, string replacement)
    {
        if (uri == null)
        {
            return null;
        }
        return Regex.Replace(uri, "^(https?://)\\*(\\.[a-z0-9\\-]+\\.[a-z]+)", $"$1{replacement}$2", RegexOptions.IgnoreCase);
    }

    private static Client GetSPA(string name, ClientDefinition definition)
    {
        var wildcardReplacement = FindWildcardReplacement(definition.RedirectUri, definition.LogoutUri);
        var redirectUriWithoutWildcard = ReplaceWildcard(definition.RedirectUri, wildcardReplacement);
        if (redirectUriWithoutWildcard == null ||
            !Uri.TryCreate(redirectUriWithoutWildcard, UriKind.Absolute, out var redirectUri))
        {
            throw new InvalidOperationException($"The redirect uri " +
                $"'{definition.RedirectUri}' for '{name}' is invalid. " +
                $"The redirect URI must be an absolute url.");
        }

        var logoutUriWithoutWildcard = ReplaceWildcard(definition.LogoutUri, wildcardReplacement);
        if (logoutUriWithoutWildcard == null ||
            !Uri.TryCreate(logoutUriWithoutWildcard, UriKind.Absolute, out var postLogouturi))
        {
            throw new InvalidOperationException($"The logout uri " +
                $"'{definition.LogoutUri}' for '{name}' is invalid. " +
                $"The logout URI must be an absolute url.");
        }

        if (!string.Equals(
            redirectUri.GetLeftPart(UriPartial.Authority),
            postLogouturi.GetLeftPart(UriPartial.Authority),
            StringComparison.Ordinal))
        {
            throw new InvalidOperationException($"The redirect uri and the logout uri " +
                $"for '{name}' have a different scheme, host or port.");
        }

        var client = ClientBuilder.SPA(name)
            .WithRedirectUri(definition.RedirectUri)
            .WithLogoutRedirectUri(definition.LogoutUri)
            .WithAllowedOrigins(redirectUri.GetLeftPart(UriPartial.Authority).Replace(wildcardReplacement, "*"))
            .FromConfiguration();

        return client.Build();
    }

    private static Client GetNativeApp(string name)
    {
        var client = ClientBuilder.NativeApp(name)
            .FromConfiguration();
        return client.Build();
    }

    private static Client GetLocalSPA(string name, ClientDefinition definition)
    {
        var client = ClientBuilder
            .IdentityServerSPA(name)
            .WithRedirectUri(definition.RedirectUri ?? DefaultLocalSPARelativeRedirectUri)
            .WithLogoutRedirectUri(definition.LogoutUri ?? DefaultLocalSPARelativePostLogoutRedirectUri)
            .WithAllowedOrigins()
            .FromConfiguration();

        return client.Build();
    }
}
