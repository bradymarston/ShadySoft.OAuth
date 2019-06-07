using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.MicrosoftAccount
{
    public class MicrosoftAccountOptions : OAuthAgentOptions
    {
        /// <summary>
        /// Initializes a new <see cref="MicrosoftAccountOptions"/>.
        /// </summary>
        public MicrosoftAccountOptions()
        {
            AuthorizationEndpoint = MicrosoftAccountDefaults.AuthorizationEndpoint;
            TokenEndpoint = MicrosoftAccountDefaults.TokenEndpoint;
            UserInformationEndpoint = MicrosoftAccountDefaults.UserInformationEndpoint;
            ProviderId = MicrosoftAccountDefaults.Id;
            ProviderDisplayName = MicrosoftAccountDefaults.DisplayName;
            Scope.Add("https://graph.microsoft.com/user.read");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "givenName");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "surname");
            ClaimActions.MapCustomJson(ClaimTypes.Email, user => user.GetString("mail") ?? user.GetString("userPrincipalName"));
        }
    }
}
