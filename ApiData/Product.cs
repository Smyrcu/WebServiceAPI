using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiData.Model;
using Dapper;

namespace ApiData
{
    public static class Product
    {
        private static string _connectionString =
            "Server=51.38.135.127,49170;Database=Mennica;User Id=sa;Password=Nie!Mam.Pomyslu#;Trusted_Connection=True;";
        public static List<ProductModel> GetProducts(string type = null)
        {
            var sql =
                "Select Id, Name, EAN, Price, Description, StockQty, InStock, Category, Type from Products where (@type = null or Type = @type)";
            var parameters = new { type };

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = connection.Query<ProductModel>(sql, parameters);
            connection.Close();

            return result.ToList();
        }
    }
}
