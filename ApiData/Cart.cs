using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using ApiData.Model;
using Dapper;

namespace ApiData
{
    public static class Cart
    {
        private static string _connectionString =
            "Server=51.38.135.127,49170;Database=Mennica;User Id=sa;Password=Nie!Mam.Pomyslu#;Trusted_Connection=True;";
        public static void AddToCart(CartItemModel item)
        {
            var sql =
                "Insert into CartItems(productid, customerid, qty, lastupdated, currencyid) values(@ProductId, @CustomerId, @Qty, @LastUpdated, @CurrencyId)";
            var parameters = new { item.ProductId, item.CustomerId, item.Qty, LastUpdated = DateTime.Now, item.CurrencyId };
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute(sql, parameters);
            connection.Close();
        }

        public static void RemoveFromCart(CartItemModel item)
        {
            var sql = "delete from CartItems where CustomerId = @CustomerId and ProductId = @ProductId";
            var parameters = new { item.ProductId, item.CustomerId };
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            connection.Execute(sql, parameters);
            connection.Close();
        }

        public static List<Currency> ReturnCurrency()
        {
            var sql = "Select currencyCode, currencyRate from Currency";
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = connection.Query<Currency>(sql).ToList();
            connection.Close();
            return result;
        }
    }
}
