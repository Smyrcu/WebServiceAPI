using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WebServiceAPI.Model.RequestModels;
using WebServiceAPI.Model.ResponseModels;
using Microsoft.Extensions.WebEncoders.Testing;

namespace WebServiceAPI.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class UserController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _config;
        public UserController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config; ;
        }

        [HttpPost("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(string username, string password)
        {
            

            if (!Membership.UserExists(username))
            {
                Membership.CreateUser(username, password);
                Roles.AddUserToRole(username, "Member");

                return Ok();
            }

            return BadRequest("User already exists.");
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return Unauthorized();
            var isSucceed = await _signInManager.CheckPasswordSignInAsync(user, password, lockoutOnFailure: false);
            if (isSucceed.Succeeded)
            {
                var token = GenerateJwtToken(user);

                return Ok(new { Token = token });
            }

            return Unauthorized();
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("your_secret_key");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
