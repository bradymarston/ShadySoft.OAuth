using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ShadySoft.OAuth.Facebook;
using ShadySoft.OAuth.Google;
using ShadySoft.OAuth.MicrosoftAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShadySoft.OAuth.Extensions
{
    public static partial class OAuthExtensions
    {
        public static OAuthBuilder AddOAuth(this IServiceCollection services)
        {
            services.AddTransient<OAuthService>();

            return new OAuthBuilder(services);
        }

        public static OAuthBuilder AddOAuth(this IServiceCollection services, Action<OAuthOptions> configureOptions)
        {
            services.Configure(configureOptions);
            services.AddTransient<OAuthService>();

            return new OAuthBuilder(services);
        }

        public static OAuthBuilder AddFacebook(this OAuthBuilder builder, Action<FacebookOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddTransient<FacebookAgent>();
            builder.Services.AddSingleton<IPostConfigureOptions<FacebookOptions>, OAuthAgentPostConfigureOptions<FacebookOptions>>();

            builder.Services.Configure<OAuthOptions>(options =>
            {
                options.RegisterService(FacebookDefaults.Id, typeof(FacebookAgent));
            });

            return builder;
        }

        public static OAuthBuilder AddGoogle(this OAuthBuilder builder, Action<GoogleOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddTransient<GoogleAgent>();
            builder.Services.AddSingleton<IPostConfigureOptions<GoogleOptions>, OAuthAgentPostConfigureOptions<GoogleOptions>>();

            builder.Services.Configure<OAuthOptions>(options =>
            {
                options.RegisterService(GoogleDefaults.Id, typeof(GoogleAgent));
            });

            return builder;
        }

        public static OAuthBuilder AddMicrosoftAccount(this OAuthBuilder builder, Action<MicrosoftAccountOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddTransient<MicrosoftAccountAgent>();
            builder.Services.AddSingleton<IPostConfigureOptions<MicrosoftAccountOptions>, OAuthAgentPostConfigureOptions<MicrosoftAccountOptions>>();

            builder.Services.Configure<OAuthOptions>(options =>
            {
                options.RegisterService(MicrosoftAccountDefaults.Id, typeof(MicrosoftAccountAgent));
            });

            return builder;
        }
    }
}
