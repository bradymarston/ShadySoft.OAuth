using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.MicrosoftAccount
{
    public class MicrosoftAccountDefaults
    {
        public const string Id = "Microsoft";
        public const string DisplayName = "Microsoft Account";

        // https://developer.microsoft.com/en-us/graph/docs/concepts/auth_v2_user
        public const string AuthorizationEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
        public const string TokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        public const string UserInformationEndpoint = "https://graph.microsoft.com/v1.0/me";

    }
}
