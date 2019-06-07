using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.Google
{
    public class GoogleAgent : OAuthAgent<GoogleOptions>
    {
        public GoogleAgent(IOptionsMonitor<GoogleOptions> agentOptionsMonitor, IOptionsMonitor<OAuthOptions> serviceOptionsMonitor)
            : base(agentOptionsMonitor, serviceOptionsMonitor)
        {
        }

        public override string BuildChallengeUrl()
        {
            var queryStrings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            queryStrings.Add("response_type", "code");
            queryStrings.Add("client_id", AgentOptions.ClientId);
            queryStrings.Add("redirect_uri", CallbackUri);

            AddQueryString(queryStrings, GoogleChallengeProperties.ScopeKey, FormatScope, AgentOptions.Scope);
            AddQueryString(queryStrings, GoogleChallengeProperties.AccessTypeKey, AgentOptions.AccessType);
            AddQueryString(queryStrings, GoogleChallengeProperties.IncludeGrantedScopesKey, v => v?.ToString().ToLower(), (bool?)null);

            var state = BuildState();
            queryStrings.Add("state", state);

            var authorizationEndpoint = QueryHelpers.AddQueryString(AgentOptions.AuthorizationEndpoint, queryStrings);
            return authorizationEndpoint;
        }

        private void AddQueryString<T>(
            IDictionary<string, string> queryStrings,
            string name,
            Func<T, string> formatter,
            T defaultValue)
        {
            var value = formatter(defaultValue);

            if (value != null)
            {
                queryStrings[name] = value;
            }
        }

        private void AddQueryString(
            IDictionary<string, string> queryStrings,
            string name,
            string defaultValue = null)
            => AddQueryString(queryStrings, name, x => x, defaultValue);
    }
}
