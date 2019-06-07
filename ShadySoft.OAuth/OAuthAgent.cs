using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShadySoft.OAuth
{
    public class OAuthAgent<TOptions> : IOAuthAgent where TOptions : OAuthAgentOptions, new()
    {
        private const string dataProtectorPurpose = "OAuthProtection";

        protected TOptions AgentOptions { get; }
        protected OAuthOptions ServiceOptions { get; private set; }
        protected string CallbackUri
        {
            get
            {
                if (string.IsNullOrEmpty(AgentOptions.ProviderCallbackUri))
                    return ServiceOptions.DefaultCallbackUri;

                return AgentOptions.ProviderCallbackUri;
            }
        }

        public OAuthAgent(IOptionsMonitor<TOptions> agentOptionsMonitor, IOptionsMonitor<OAuthOptions> serviceOptionsMonitor)
        {
            AgentOptions = agentOptionsMonitor.CurrentValue;
            ServiceOptions = serviceOptionsMonitor.CurrentValue;
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfo(string code, string state)
        {
            if (string.IsNullOrEmpty(state))
            {
                throw new ArgumentException(nameof(state));
            }

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentException(nameof(code));
            }

            if (!VerifyState(state))
                return null;

            var tokens = await ExchangeCodeAsync(code);

            if (tokens.Error != null)
            {
                throw tokens.Error;
            }

            if (string.IsNullOrEmpty(tokens.AccessToken))
            {
                throw new Exception("Failed to retrieve access token.");
            }

            var principal = await CreatePrincipalAsync(tokens);

            return new ExternalLoginInfo(principal, AgentOptions.ProviderId, principal.FindFirst(ClaimTypes.NameIdentifier).Value, AgentOptions.ProviderDisplayName);
        }

        protected virtual async Task<OAuthTokenResponse> ExchangeCodeAsync(string code)
        {
            var tokenRequestParameters = new Dictionary<string, string>()
            {
                { "client_id", AgentOptions.ClientId },
                { "redirect_uri", CallbackUri },
                { "client_secret", AgentOptions.ClientSecret },
                { "code", code },
                { "grant_type", "authorization_code" },
            };

            var requestContent = new FormUrlEncodedContent(tokenRequestParameters);

            var requestMessage = new HttpRequestMessage(HttpMethod.Post, AgentOptions.TokenEndpoint);
            requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            requestMessage.Content = requestContent;
            var response = await AgentOptions.Backchannel.SendAsync(requestMessage);
            if (response.IsSuccessStatusCode)
            {
                var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                return OAuthTokenResponse.Success(payload);
            }
            else
            {
                var error = "OAuth token endpoint failure: " + await Display(response);
                return OAuthTokenResponse.Failed(new Exception(error));
            }
        }

        private static async Task<string> Display(HttpResponseMessage response)
        {
            var output = new StringBuilder();
            output.Append("Status: " + response.StatusCode + ";");
            output.Append("Headers: " + response.Headers.ToString() + ";");
            output.Append("Body: " + await response.Content.ReadAsStringAsync() + ";");
            return output.ToString();
        }

        protected virtual async Task<ClaimsPrincipal> CreatePrincipalAsync(OAuthTokenResponse tokens)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, AgentOptions.UserInformationEndpoint);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            var response = await AgentOptions.Backchannel.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"An error occurred when retrieving Microsoft user information ({response.StatusCode}). Please check if the authentication information is correct and the corresponding {AgentOptions.ProviderDisplayName} API is enabled.");
            }

            using (var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync()))
            {
                var identity = new ClaimsIdentity();
                RunClaimActions(payload.RootElement, identity);
                return new ClaimsPrincipal(identity);
            }
        }

        public virtual string BuildChallengeUrl()
        {
            var scope = FormatScope();

            var parameters = new Dictionary<string, string>
            {
                { "client_id", AgentOptions.ClientId },
                { "scope", scope },
                { "response_type", "code" },
                { "redirect_uri", CallbackUri },
                { "state", BuildState() }
            };
            return QueryHelpers.AddQueryString(AgentOptions.AuthorizationEndpoint, parameters);
        }

        protected virtual string BuildState()
        {
            var loginTimeout = DateTime.UtcNow + AgentOptions.RemoteAuthenticationTimeout;

            return AgentOptions.DataProtector.Protect(JsonConvert.SerializeObject(loginTimeout));
        }

        protected virtual bool VerifyState(string state)
        {
            try
            {
                var loginTimeout = JsonConvert.DeserializeObject<DateTime>(AgentOptions.DataProtector.Unprotect(state));
                if (loginTimeout > DateTime.UtcNow)
                    return true;
            }
            catch
            {
                return false;
            }
            return false;
        }

        protected virtual string FormatScope(IEnumerable<string> scopes)
            => string.Join(" ", scopes); // OAuth2 3.3 space separated

        protected virtual string FormatScope()
            => FormatScope(AgentOptions.Scope);

        protected void RunClaimActions(JsonElement userData, ClaimsIdentity identity)
        {
            foreach (var action in AgentOptions.ClaimActions)
            {
                action.Run(userData, identity, AgentOptions.ClaimsIssuer);
            }
        }
    }
}