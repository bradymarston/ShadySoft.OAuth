using System;
using System.Text.Json;

namespace ShadySoft.OAuth.Models
{
    public class OAuthTokenResponse : IDisposable
    {
        private OAuthTokenResponse(JsonDocument response)
        {
            Response = response;
            var root = response.RootElement;
            AccessToken = GetString(root, "access_token");
            TokenType = GetString(root, "token_type");
            RefreshToken = GetString(root, "refresh_token");
            ExpiresIn = GetString(root, "expires_in");
        }

        private OAuthTokenResponse(Exception error)
        {
            Error = error;
        }

        public static OAuthTokenResponse Success(JsonDocument response)
        {
            return new OAuthTokenResponse(response);
        }

        public static OAuthTokenResponse Failed(Exception error)
        {
            return new OAuthTokenResponse(error);
        }

        public void Dispose()
        {
            Response?.Dispose();
        }

        public JsonDocument Response { get; set; }
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public string ExpiresIn { get; set; }
        public Exception Error { get; set; }

        private static string GetString(JsonElement element, string key)
        {
            if (element.TryGetProperty(key, out var property) && property.Type != JsonValueType.Null)
            {
                return property.ToString();
            }

            return null;
        }
    }
}