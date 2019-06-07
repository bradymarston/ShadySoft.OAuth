using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.Facebook
{
    public class FacebookOptions : OAuthAgentOptions
    {
        /// <summary>
        /// Initializes a new <see cref="FacebookOptions"/>.
        /// </summary>
        public FacebookOptions()
        {
            SendAppSecretProof = true;
            AuthorizationEndpoint = FacebookDefaults.AuthorizationEndpoint;
            TokenEndpoint = FacebookDefaults.TokenEndpoint;
            UserInformationEndpoint = FacebookDefaults.UserInformationEndpoint;
            ProviderId = FacebookDefaults.Id;
            ProviderDisplayName = FacebookDefaults.DisplayName;
            
            Scope.Add("email");
            Fields.Add("name");
            Fields.Add("email");
            Fields.Add("first_name");
            Fields.Add("last_name");

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
            ClaimActions.MapJsonSubKey("urn:facebook:age_range_min", "age_range", "min");
            ClaimActions.MapJsonSubKey("urn:facebook:age_range_max", "age_range", "max");
            ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
            ClaimActions.MapJsonKey("urn:facebook:middle_name", "middle_name");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
            ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
            ClaimActions.MapJsonKey("urn:facebook:link", "link");
            ClaimActions.MapJsonSubKey("urn:facebook:location", "location", "name");
            ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");
            ClaimActions.MapJsonKey("urn:facebook:timezone", "timezone");
        }

        /// <summary>
        /// Check that the options are valid.  Should throw an exception if things are not ok.
        /// </summary>
        public override void Validate(OAuthOptions oAuthOptions)
        {
            if (string.IsNullOrEmpty(AppId))
            {
                throw new ArgumentException(nameof(AppId));
            }

            if (string.IsNullOrEmpty(AppSecret))
            {
                throw new ArgumentException(nameof(AppSecret));
            }

            if (string.IsNullOrEmpty(AppSecret))
            {
                throw new ArgumentException(nameof(AppSecret));
            }

            base.Validate(oAuthOptions);
        }

        // Facebook uses a non-standard term for this field.
        /// <summary>
        /// Gets or sets the Facebook-assigned appId.
        /// </summary>
        public string AppId
        {
            get { return ClientId; }
            set { ClientId = value; }
        }

        // Facebook uses a non-standard term for this field.
        /// <summary>
        /// Gets or sets the Facebook-assigned app secret.
        /// </summary>
        public string AppSecret
        {
            get { return ClientSecret; }
            set { ClientSecret = value; }
        }

        /// <summary>
        /// Gets or sets if the appsecret_proof should be generated and sent with Facebook API calls.
        /// This is enabled by default.
        /// </summary>
        public bool SendAppSecretProof { get; set; }

        /// <summary>
        /// The list of fields to retrieve from the UserInformationEndpoint.
        /// https://developers.facebook.com/docs/graph-api/reference/user
        /// </summary>
        public ICollection<string> Fields { get; } = new HashSet<string>();
    }
}