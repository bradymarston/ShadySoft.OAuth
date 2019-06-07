using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.DataProtection;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShadySoft.OAuth
{
    public class OAuthAgentOptions
    {
        /// <summary>

        /// Check that the options are valid.  Should throw an exception if things are not ok.

        /// </summary>

        public virtual void Validate(OAuthOptions oAuthOptions)
        {
            if (string.IsNullOrEmpty(ClientId))
            {
                throw new ArgumentException(nameof(ClientId));
            }

            if (string.IsNullOrEmpty(ClientSecret))
            {
                throw new ArgumentException(nameof(ClientSecret));
            }

            if (string.IsNullOrEmpty(AuthorizationEndpoint))
            {
                throw new ArgumentException(nameof(AuthorizationEndpoint));
            }

            if (string.IsNullOrEmpty(TokenEndpoint))
            {
                throw new ArgumentException(nameof(TokenEndpoint));
            }

            if (string.IsNullOrEmpty(ProviderCallbackUri) && string.IsNullOrEmpty(oAuthOptions.DefaultCallbackUri))
            {
                throw new ArgumentException("If no OAuthOptions.DefaultCallbackUri exists, each agent must supply a CallbackUri in its options.", nameof(ProviderCallbackUri));
            }

            if (string.IsNullOrEmpty(ProviderId))
            {
                throw new ArgumentException(nameof(ProviderId));
            }

            if (string.IsNullOrEmpty(ProviderDisplayName))
            {
                throw new ArgumentException(nameof(ProviderDisplayName));
            }
        }

        /// <summary>
        /// Gets or sets the type used to secure data.
        /// </summary>
        public IDataProtector DataProtector { get; set; }

        /// <summary>
        /// Gets or sets timeout value in milliseconds for back channel communications with the remote identity provider.
        /// </summary>
        /// <value>
        /// The back channel timeout.
        /// </value>
        public TimeSpan BackchannelTimeout { get; set; } = TimeSpan.FromSeconds(60);

        /// <summary>
        /// The HttpMessageHandler used to communicate with remote identity provider.
        /// This cannot be set at the same time as BackchannelCertificateValidator unless the value 
        /// can be downcast to a WebRequestHandler.
        /// </summary>
        public HttpMessageHandler BackchannelHttpHandler { get; set; }

        /// <summary>
        /// The HttpMessageHandler used to communicate with remote identity provider.
        /// This cannot be set at the same time as BackchannelCertificateValidator unless the value 
        /// can be downcast to a WebRequestHandler.
        /// </summary>
        public HttpClient Backchannel { get; set;  }

        /// <summary>
        /// Gets or sets the provider-assigned client id.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets the provider-assigned client secret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets the URI where the client will be redirected to authenticate.
        /// </summary>
        public string AuthorizationEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to exchange the OAuth token.
        /// </summary>
        public string TokenEndpoint { get; set; }

        /// <summary>
        /// Gets or sets the URI the middleware will access to obtain the user information.
        /// This value is not used in the default implementation, it is for use in custom implementations of
        /// IOAuthAuthenticationEvents.Authenticated or OAuthAuthenticationHandler.CreateTicketAsync.
        /// </summary>
        public string UserInformationEndpoint { get; set; }

        /// <summary>
        /// The url to where the external service will return.
        /// </summary>
        public string ProviderCallbackUri { get; set; }

        /// <summary>
        /// Gets or sets the time limit for completing the authentication flow (15 minutes by default).
        /// </summary>
        public TimeSpan RemoteAuthenticationTimeout { get; set; } = TimeSpan.FromMinutes(15);

        /// <summary>
        /// Gets or sets the external login provider id
        /// </summary>
        public string ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the external login provider display name
        /// </summary>
        public string ProviderDisplayName { get; set; }

        /// <summary>
        /// Gets the issuer that should be used for any claims that are created
        /// </summary>
        public string ClaimsIssuer => ProviderId;

        /// <summary>
        /// A collection of claim actions used to select values from the json user data and create Claims.
        /// </summary>
        public ClaimActionCollection ClaimActions { get; } = new ClaimActionCollection();

        /// <summary>
        /// Gets the list of permissions to request.
        /// </summary>
        public ICollection<string> Scope { get; } = new HashSet<string>();
    }
}