using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.Google
{
    public class GoogleChallengeProperties
    {
        /// <summary>
        /// The parameter key for the "scope" argument being used for a challenge request.
        /// </summary>
        public static readonly string ScopeKey = "scope";

        /// <summary>
        /// The parameter key for the "access_type" argument being used for a challenge request.
        /// </summary>
        public static readonly string AccessTypeKey = "access_type";

        /// <summary>
        /// The parameter key for the "approval_prompt" argument being used for a challenge request.
        /// </summary>
        public static readonly string ApprovalPromptKey = "approval_prompt";

        /// <summary>
        /// The parameter key for the "include_granted_scopes" argument being used for a challenge request.
        /// </summary>
        public static readonly string IncludeGrantedScopesKey = "include_granted_scopes";

        /// <summary>
        /// The parameter key for the "login_hint" argument being used for a challenge request.
        /// </summary>
        public static readonly string LoginHintKey = "login_hint";

        /// <summary>
        /// The parameter key for the "prompt" argument being used for a challenge request.
        /// </summary>
        public static readonly string PromptParameterKey = "prompt";
    }
}
