using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using ApiData.Model;
using Dapper;
using Newtonsoft.Json;

namespace ApiData
{
    public static class UserLogin
    {
        private const string ConnectionString = "Server=51.38.135.127,49170;Database=Mennica;User Id=sa;Password=Nie!Mam.Pomyslu#;Trusted_Connection=False;";

        public static string CreateUser(UserModel model)
        {
            var createSql = "INSERT INTO Users (UserName, Password, Email) VALUES (@Username, @Password, @Email)";
            var createParameters = new {model.Username, model.Password, model.Email};
            
            var sql2 = "Select UserId from Users where Username = @User";
            var parameters2 = new { User = model.Username };

            var connection = new SqlConnection(ConnectionString);

            connection.Open();
            connection.Execute(createSql, createParameters);
            var userId = connection.QueryFirst<string>(sql2, parameters2);


            var sql = "INSERT INTO UserExtraInfo (UserId, Name, Surname, BirthDate) VALUES (@userId, @Name, @Surname, @BirthDate)";
            var parameters = new { UserId = userId, model.Name, model.Surname, BirthDate = model.BirthDate.ToString("yyyy-MM-dd") };
            connection.Execute(sql, parameters);
            connection.Close();
            return userId;
        }

        public static int? LoginUser(LoginModel model)
        {
            var sql = "SELECT UserId FROM Users WHERE UserName = @Username AND Password = @Password";
            var parameters = new { model.Username, model.Password };

            var connection = new SqlConnection(ConnectionString);
            connection.Open();
            var userId = connection.QueryFirstOrDefault<int>(sql, parameters);
            connection.Close();
            
            return userId;
        }

        public static bool UpdateUser(UserModel model)
        {
            var sql = @"UPDATE u set u.Name = @Name, u.BirthDate = @BirthDate, u.Surname = @Surname 
                        from UserExtraInfo u where u.UserId = @UserId";
            var parameters = new { model.Name, BirthDate = model.BirthDate.ToString("yyyy-MM-dd"), model.Surname, model.UserId };

            var connection = new SqlConnection(ConnectionString);
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
                client.BaseAddress = new Uri("https://api.nbp.pl/api/exchangerates/");

                var response = client.GetAsync("tables/A").Result;
                var res = response.Content.ReadAsStringAsync().Result;
                var resList = JsonConvert.DeserializeObject<List<CurrencyResponse>>(res);
                result = resList[0];
            }
            var connection = new SqlConnection(ConnectionString);
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
