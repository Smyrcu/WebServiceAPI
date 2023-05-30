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
            "Server=51.38.135.127,49170;Database=Mennica;User Id=sa;Password=Nie!Mam.Pomyslu#;Trusted_Connection=False;";

        public static List<ProductModel> GetProducts(string type = null)
        {
            var sql =
                "Select Id, Name, EAN, Price, Description, StockQty, InStock, Category, Type from Products where (coalesce(@type, '') = '' or Type = @type)";
            var parameters = new { type };

            var connection = new SqlConnection(_connectionString);
            connection.Open();
            var result = connection.Query<ProductModel>(sql, parameters);
            connection.Close();

            return result.ToList();
        }

        public static void PlaceOrder(PlaceOrderModel model)
        {
            var sql =
                "Insert Into CustomerOrder (UserId, ShippingCity, ShippingPostalNumber, ShippingCountry, ShippingAdress, PaymentId, ShippingMethodId, Price) VALUES (@UserId, @ShippingCity, @ShippingPostalNumber, @ShippingCountry, @ShippingAdress, @PaymentId, @ShippingMethodId, @Price)";
            var parameters = new
            {
                model.CustomerOrder.UserId, model.CustomerOrder.ShippingCity, model.CustomerOrder.ShippingPostalNumber,
                model.CustomerOrder.ShippingCountry,
                ShippingAddress = model.CustomerOrder.ShippingAdress, model.CustomerOrder.PaymentId,
                model.CustomerOrder.ShippingMethodId, model.CustomerOrder.Price
            };
            var connection = new SqlConnection(_connectionString);
            var sql2 = "Select top 1 Id from CustomerOrder order by Id desc";
            var sql3 =
                "INSERT INTO CustomerOrderItem (CustomerOrderId, ProductId, Qty) VALUES (@CustomerOrderId, @ProductId, @Qty)";
            connection.Open();
            var CustomerOrderId = connection.Query<int>(sql2);
            connection.Execute(sql, parameters);
            foreach (var cartItemModel in model.CartItemModel)
            {
                var parameters3 = new { CustomerOrderId, cartItemModel.ProductId, cartItemModel.Qty };
                connection.Execute(sql3, parameters3);
            }

            connection.Close();

        }

        public static List<OrderHistoryModel> GetOrderHistory(string UserId)
        {
            var sql =
                "Select co.Id, co.UserId, co.ShippingCity, co.ShippingPostalNumber, co.ShippingCountry, co.ShippingAdress, co.PaymentId, co.ShippingMethodId, co.Price from CustomerOrder co where UserId = @UserId order by Id desc";
            var parameters = new { UserId };
            var connection = new SqlConnection(_connectionString);
            var result = new List<OrderHistoryModel>();
            connection.Open();
            var response = connection.Query<CustomerOrder>(sql, parameters).ToList();
            foreach (var order in response)
            {
                var sql2 = "SELECT coi.ProductId, coi.Qty FROM CustomerOrderItem coi where coi.CustomerOrderId = @Id";
                var parameters2 = new { UserId };
                var productList = connection.Query<CartItemModel>(sql2, parameters2).ToList();
                result.Add(new OrderHistoryModel
                {
                    CustomerOrder = order,
                    CartItemModel = productList
                });
            }

            connection.Close();

            return result;
        }
    }
}
