﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using Dapper;
using Data.Model;

namespace Data
{
    public static class UserLogin
    {
        private static string _connectionString =
            "Server=51.38.135.127,49170;Database=Mennica;User Id=sa;Password=Nie!Mam.Pomyslu#;Trusted_Connection=True;";
        public static string CreateUser(UserModel model)
        {
            Membership.CreateUser(model.Username, model.Password, model.Email);

            var sql2 = "Select UserId from aspnet_Users where Username = @User";
            var parameters2 = new { User = model.Username };

            var connection = new SqlConnection(_connectionString);

            connection.Open();
            var UserId = connection.QueryFirst<string>(sql2, parameters2);
            

            var sql = "INSERT INTO UserExtraInfo (UserId, Name, Surname, BirthDate) VALUES (@UserId, @Name, @Surname, @BirthDate)";
            var parameters = new { UserId, model.Name, model.Surname, BirthDate = model.BirthDate.ToString("yyyy-MM-dd") };

            Roles.AddUserToRole(model.Username, "User");
            return UserId;
        }

        public static bool LoginUser(LoginModel model)
        {
            return Membership.ValidateUser(model.Username, model.Password);
        }
    }
}
