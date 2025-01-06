using Duende.IdentityServer.Configuration;
using Duende.IdentityServer.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Cynosura.IdentityServer.Extensions
{
    public class CustomClientConfigurationValidator : DefaultClientConfigurationValidator
    {
        public CustomClientConfigurationValidator(IdentityServerOptions options) : base(options)
        {
        }

        private static string ReplaceWildcard(string uri, string replacement)
        {
            if (uri == null)
            {
                return null;
            }
            return Regex.Replace(uri, "^(https?://)\\*(\\.[a-z0-9\\-]+\\.[a-z]+)", $"$1{replacement}$2", RegexOptions.IgnoreCase);
        }

        protected override Task ValidateAllowedCorsOriginsAsync(ClientConfigurationValidationContext context)
        {
            if (context.Client.AllowedCorsOrigins?.Any() == true)
            {
                foreach (var origin in context.Client.AllowedCorsOrigins)
                {
                    var originWithoutWildCard = ReplaceWildcard(origin, "w");
                    var fail = true;

                    if (!string.IsNullOrWhiteSpace(originWithoutWildCard) && IsUri(originWithoutWildCard))
                    {
                        var uri = new Uri(originWithoutWildCard);

                        if (uri.AbsolutePath == "/" && !origin.EndsWith("/"))
                        {
                            fail = false;
                        }
                    }

                    if (fail)
                    {
                        if (!string.IsNullOrWhiteSpace(origin))
                        {
                            context.SetError($"AllowedCorsOrigins contains invalid origin: {origin}");
                        }
                        else
                        {
                            context.SetError($"AllowedCorsOrigins contains invalid origin. There is an empty value.");
                        }
                        return Task.CompletedTask;
                    }
                }
            }

            return Task.CompletedTask;
        }

        public static bool IsUri(string input)
        {
            if (!Uri.TryCreate(input, UriKind.Absolute, out var uri))
            {
                return false;
            }

            if (uri.IsFile && !input.StartsWith(Uri.UriSchemeFile + "://", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
