using AmSoul.Core.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace VueJSDotnet51.Services
{
    public class AuthService
    {
        private readonly JwtTokenOptions _tokenOptions;
        //private readonly UserManager<User> _userManager;
        //private readonly RoleManager<Role> _roleManager;
        //private readonly SignInManager<User> _signInManager;
        //private readonly ILogger<AuthService> _logger;
        //private readonly string _wwwRootPath;
        public AuthService(JwtTokenOptions tokenOptions
            //, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, ILogger<AuthService> logger, IWebHostEnvironment hostEnviroment
            )
        {
            _tokenOptions = tokenOptions;
            //_userManager = userManager;
            //_roleManager = roleManager;
            //_signInManager = signInManager;
            //_logger = logger;
            //_wwwRootPath = hostEnviroment.WebRootPath;
        }
        public string BuildToken(string userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userId) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey)), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }



    }
}
