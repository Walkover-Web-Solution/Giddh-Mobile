using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giddh_Cross_Portable
{
    public class OAuthSettings
    {        

        public OAuthSettings(
            string clientId,
            string scope,
            string authorizeUrl,
            string redirectUrl,
            string clientSecret,
            string refreshToken)
        {
            ClientId = clientId;
            Scope = scope;
            AuthorizeUrl = authorizeUrl;
            RedirectUrl = redirectUrl;
            ClientSecret = clientSecret;
            RefreshToken = refreshToken;
        }

        public OAuthSettings(
            string clientId,
            string clientSecret,
            string authorizeUrl,
            string accessTokenUrl,
            string requestTokenUrl)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            AuthorizeUrl = authorizeUrl;
            AccessTokenUrl = accessTokenUrl;
            RequestTokenUrl = requestTokenUrl;
            RedirectUrl = "http://www.google.com";
        }

        public string ClientId { get; private set; }
        public string Scope { get; private set; }
        public string AuthorizeUrl { get; private set; }
        public string RedirectUrl { get; private set; }
        public string ClientSecret { get; private set; }
        public string RefreshToken { get; private set; }

        public string AccessTokenUrl { get; private set; }
        public string RequestTokenUrl { get; private set; }

    }
}
