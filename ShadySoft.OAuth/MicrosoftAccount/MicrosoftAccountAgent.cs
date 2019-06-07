using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.MicrosoftAccount
{
    public class MicrosoftAccountAgent : OAuthAgent<MicrosoftAccountOptions>
    {
        public MicrosoftAccountAgent(IOptionsMonitor<MicrosoftAccountOptions> agentOptionsMonitor, IOptionsMonitor<OAuthOptions> serviceOptionsMonitor)
            : base(agentOptionsMonitor, serviceOptionsMonitor)
        {
        }
    }
}
