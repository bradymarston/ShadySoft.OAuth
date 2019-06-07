using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.Facebook
{
    public static class FacebookDefaults
    {
        public const string Id = "Facebook";
        public const string DisplayName = "Facebook";

        // https://developers.facebook.com/docs/facebook-login/manually-build-a-login-flow#login
        public const string AuthorizationEndpoint = "https://www.facebook.com/v3.1/dialog/oauth";
        public const string TokenEndpoint = "https://graph.facebook.com/v3.1/oauth/access_token";
        public const string UserInformationEndpoint = "https://graph.facebook.com/v3.1/me";
    }
}
