using Giddh_Cross_Portable.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Auth;

namespace Giddh_Cross_Portable.iOS.useInterface
{
    public class callAuth : ICallAuth
    {
        public async void Auth(object getThis)
        {
            //OAuth2Authenticator auth;
        }

        public void twitterLogin()
        {

        }

        public void logout()
        {
            Constants.userObj.authKey = string.Empty;
        }
    }
}
