using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.Google
{
    public class GoogleDefaults
    {
        public const string Id = "Google";
        public const string DisplayName = "Google";

        // https://developers.google.com/identity/protocols/OAuth2WebServer
        public const string AuthorizationEndpoint = "https://accounts.google.com/o/oauth2/v2/auth";
        public const string TokenEndpoint = "https://www.googleapis.com/oauth2/v4/token";
        public const string UserInformationEndpoint = "https://www.googleapis.com/oauth2/v2/userinfo";
    }
}
