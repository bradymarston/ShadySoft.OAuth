using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShadySoft.OAuth
{
    public class OAuthOptions
    {
        private IDictionary<string, Type> _serviceRegistry = new Dictionary<string, Type>();

        public string DefaultCallbackUri { get; set; }

        public IReadOnlyDictionary<string, Type> ServiceRegistry
        {
            get
            {
                return (IReadOnlyDictionary<string, Type>)_serviceRegistry;
            }
        }

        public void RegisterService(string provider, Type service)
        {
            _serviceRegistry.Add(provider, service);
        }
    }
}
