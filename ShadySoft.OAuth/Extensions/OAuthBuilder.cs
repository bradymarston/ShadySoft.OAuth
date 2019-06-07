using Microsoft.Extensions.DependencyInjection;

namespace ShadySoft.OAuth.Extensions
{
    public static partial class OAuthExtensions
    {
        public class OAuthBuilder
        {
            public IServiceCollection Services { get;  }

            public OAuthBuilder(IServiceCollection services)
            {
                Services = services;
            }
        }
    }
}
