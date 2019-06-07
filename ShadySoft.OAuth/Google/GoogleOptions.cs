using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.Google
{
    public class GoogleOptions : OAuthAgentOptions
    {
        /// <summary>
        /// Initializes a new <see cref="GoogleOptions"/>.
        /// </summary>
        public GoogleOptions()
        {
            AuthorizationEndpoint = GoogleDefaults.AuthorizationEndpoint;
            TokenEndpoint = GoogleDefaults.TokenEndpoint;
            UserInformationEndpoint = GoogleDefaults.UserInformationEndpoint;
            ProviderId = GoogleDefaults.Id;
            ProviderDisplayName = GoogleDefaults.DisplayName;

            Scope.Add("openid");
            Scope.Add("profile");
            Scope.Add("email");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
            ClaimActions.MapJsonKey("urn:google:profile", "link");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
        }

        /// <summary>
        /// access_type. Set to 'offline' to request a refresh token.
        /// </summary>
        public string AccessType { get; set; }
    }
}
