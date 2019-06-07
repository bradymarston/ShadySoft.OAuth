using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.Facebook
{
    public class FacebookAgent : OAuthAgent<FacebookOptions>
    {
        public FacebookAgent(IOptionsMonitor<FacebookOptions> agentOptionsMonitor, IOptionsMonitor<OAuthOptions> serviceOptionsMonitor) 
            : base(agentOptionsMonitor, serviceOptionsMonitor)
        {
        }

        protected override async Task<ClaimsPrincipal> CreatePrincipalAsync(OAuthTokenResponse tokens)
        {
            var endpoint = QueryHelpers.AddQueryString(AgentOptions.UserInformationEndpoint, "access_token", tokens.AccessToken);
            if (AgentOptions.SendAppSecretProof)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "appsecret_proof", GenerateAppSecretProof(tokens.AccessToken));
            }
            if (AgentOptions.Fields.Count > 0)
            {
                endpoint = QueryHelpers.AddQueryString(endpoint, "fields", string.Join(",", AgentOptions.Fields));
            }

            var response = await AgentOptions.Backchannel.GetAsync(endpoint);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Facebook user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding Facebook Graph API is enabled.");
            }

            using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
            {
                var identity = new ClaimsIdentity();
                RunClaimActions(payload.RootElement, identity);
                return new ClaimsPrincipal(identity);
            }
        }

        private string GenerateAppSecretProof(string accessToken)
        {
            using (var algorithm = new HMACSHA256(Encoding.ASCII.GetBytes(AgentOptions.AppSecret)))
            {
                var hash = algorithm.ComputeHash(Encoding.ASCII.GetBytes(accessToken));
                var builder = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    builder.Append(hash[i].ToString("x2", CultureInfo.InvariantCulture));
                }
                return builder.ToString();
            }
        }

        protected override string FormatScope(IEnumerable<string> scopes)
        {
            // Facebook deviates from the OAuth spec here. They require comma separated instead of space separated.
            // https://developers.facebook.com/docs/reference/dialogs/oauth
            // http://tools.ietf.org/html/rfc6749#section-3.3
            return string.Join(",", scopes);
        }
    }
}