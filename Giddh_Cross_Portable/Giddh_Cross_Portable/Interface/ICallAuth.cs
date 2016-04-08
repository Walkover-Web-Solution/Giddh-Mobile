using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Giddh_Cross_Portable.Interface
{
    public interface ICallAuth
    {
		void Auth(object passthis);

        void twitterLogin();

        void logout();
    }
}
