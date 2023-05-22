using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace test
{
    public static class UserLogin
    {
        public static string CreateUser()
        {
            Membership.CreateUser("", "");
            return "1";
        }
    }
}
