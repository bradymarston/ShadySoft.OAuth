using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShadySoft.OAuth
{
    public class OAuthService
    {
        private readonly IServiceProvider _serviceProvider;
        public OAuthOptions Options;

        public OAuthService(IOptionsMonitor<OAuthOptions> optionsMonitor, IServiceProvider serviceProvider)
        {
            Options = optionsMonitor.CurrentValue;
            _serviceProvider = serviceProvider;
        }

        public string BuildChallengeUrl(string provider)
        {
            return GetProviderService(provider).BuildChallengeUrl();
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfo(string provider, string code, string state)
        {
            return await GetProviderService(provider).GetExternalLoginInfo(code, state);
        }

        private IOAuthAgent GetProviderService(string provider)
        {
            return (IOAuthAgent)_serviceProvider.GetService(Options.ServiceRegistry[provider]);
        }
    }
}
