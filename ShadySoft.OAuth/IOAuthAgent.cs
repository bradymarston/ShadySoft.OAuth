using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ShadySoft.OAuth
{
    public interface IOAuthAgent
    {
        string BuildChallengeUrl();
        Task<ExternalLoginInfo> GetExternalLoginInfo(string code, string state);
    }
}