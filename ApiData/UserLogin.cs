﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using ApiData.Model;
using Dapper;
using Newtonsoft.Json;
using static System.Net.WebRequestMethods;

namespace ApiData
{
    public static class UserLogin
    {
        private static string _connectionString =
            "Server=51.38.135.127,49170;Database=Mennica;User Id=sa;Password=Nie!Mam.Pomyslu#;Trusted_Connection=False;";
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

        public static bool UpdateUser(UserModel model)
        {
            var sql = @"UPDATE u set u.Name = @Name, u.BirthDate = @BirthDate, u.Surname = @Surname 
                        from UserExtraInfo u where u.UserId = @UserId";
            var parameters = new { model.Name, model.BirthDate, model.Surname, model.UserId };

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute(sql, parameters);
            connection.Close();

            return true;
        }

        public static void SaveCurrency()
        {
            CurrencyResponse result;
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://api.nbp.pl/api/exchangerates/");

                var response = client.GetAsync("tables/A").Result;
                var res = response.Content.ReadAsStringAsync().Result;
                var resList = JsonConvert.DeserializeObject<List<CurrencyResponse>>(res);
                result = resList[0];
            }
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            foreach (var rate in result.rates)
            {
                var sql = "INSERT INTO Currency (currencyCode, currencyRate) VALUES (@code, @mid)";

                var parameters = new { rate.code, rate.mid };
                connection.Execute(sql, parameters);
            }
            connection.Close();

        }

       
        
    }
}