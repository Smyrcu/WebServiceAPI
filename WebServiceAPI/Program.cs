using System;
using System.Text;
using ApiData;
using ApiData.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "your_issuer",
            ValidAudience = "your_audience",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your_secret_key"))
        };
    });





var app = builder.Build();

app.MapGet("test", () => "Schludnie");

app.MapGet("test/curr", () => UserLogin.SaveCurrency());

app.MapPost("user/create", UserLogin.CreateUser);

app.MapPost("user/login", ([FromBody] LoginModel model) =>
{
    return UserLogin.LoginUser(model);
});

app.MapPost("user/update", ([FromBody] UserModel model) =>
{
    return UserLogin.UpdateUser(model);
});

app.MapGet("products/{type}", ([FromRoute] string type ) =>
{
    return Product.GetProducts(type);
});

app.MapGet("products", () => Product.GetProducts());

app.MapPost("cart/add", ([FromBody] CartItemModel model) =>
{
    Cart.AddToCart(model);
});

app.MapPost("cart/remove", ([FromBody] CartItemModel model) =>
{
    Cart.RemoveFromCart(model);
});

app.MapGet("cart/{UserId}",([FromRoute] string UserId) =>
{
    Cart.GetCartItems(UserId);
});

app.MapPost("order/placeOrder", ([FromBody] PlaceOrderModel model) =>
{
    Product.PlaceOrder(model);
});

app.MapGet("user/history/{UserId}", ([FromRoute] string UserId) =>
{
    Product.GetOrderHistory(UserId);
});

app.MapPost("pay/{UserId}/{OrderId}", ([FromRoute] string UserId, [FromRoute] string OrderId) =>
{
    return "Order paid";
});

app.MapGet("currency", () => Cart.ReturnCurrency());


app.Run();