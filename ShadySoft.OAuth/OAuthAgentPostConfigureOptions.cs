using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ShadySoft.OAuth
{
    public class OAuthAgentPostConfigureOptions<TOptions> : IPostConfigureOptions<TOptions> where TOptions : OAuthAgentOptions, new()
    {
        private readonly IDataProtectionProvider _dp;
        private readonly OAuthOptions _oAuthOptions;

        public OAuthAgentPostConfigureOptions(IDataProtectionProvider dataProtection, IOptionsMonitor<OAuthOptions> optionsMonitor)
        {
            _dp = dataProtection;
            _oAuthOptions = optionsMonitor.CurrentValue;
        }

        public void PostConfigure(string name, TOptions options)
        {
            options.DataProtector = options.DataProtector ?? _dp.CreateProtector($"State strings for {options.ProviderDisplayName}");

            if (options.Backchannel == null)
            {
                options.Backchannel = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler());
                options.Backchannel.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth handler");
                options.Backchannel.Timeout = options.BackchannelTimeout;
                options.Backchannel.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB
            }

            options.Validate(_oAuthOptions);
        }
    }
}
