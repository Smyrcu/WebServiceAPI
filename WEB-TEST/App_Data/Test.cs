using System.Web.Security;

namespace WEB_TEST.App_Data
{
    public static class Test
    {

        public static string Testowa()
        {
            Membership.CreateUser("test", "Testowe1@");
            Roles.CreateRole("test");
            Roles.AddUserToRole("test", "test");
            
            return "";
        }
    }
}