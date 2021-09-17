using AmSoul.Core.Interfaces;
using AmSoul.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AmSoul.Core.Services
{
    public class UserServiceBase : UserServiceBase<BaseUser, BaseRole>, IUserService
    {
        public UserServiceBase(
            JwtTokenOptions tokenOptions,
            UserManager<BaseUser> userManager,
            RoleManager<BaseRole> roleManager,
            ILogger<UserServiceBase> logger)
            : base(tokenOptions, userManager, roleManager, logger) { }
    }

    public abstract class UserServiceBase<TUser, TRole> : IUserService<TUser, TRole>
        where TUser : BaseUser
        where TRole : BaseRole
    {
        private readonly JwtTokenOptions _tokenOptions;
        private readonly UserManager<TUser> _userManager;
        private readonly RoleManager<TRole> _roleManager;
        private readonly ILogger _logger;

        public UserServiceBase(JwtTokenOptions tokenOptions, UserManager<TUser> userManager, RoleManager<TRole> roleManager, ILogger<UserServiceBase<TUser, TRole>> logger)
        {
            _tokenOptions = tokenOptions;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        public virtual async Task<IdentityResult> CreateAsync(TUser user, string password, List<string> roles)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                result = await _userManager.AddToRolesAsync(user, roles);
                _logger.Log(LogLevel.Information, $"User {user.UserName} Created Success");
            }
            return result;
        }
        public virtual async Task<IdentityResult> CreateRoleAsync(TRole role) => await _roleManager.CreateAsync(role);
        public virtual IQueryable<TUser> GetUsers() => _userManager.Users;
        public virtual IQueryable<TRole> GetRoles() => _roleManager.Roles;
        public virtual async Task<TUser> GetUserByIdAsync(string id) => await _userManager.FindByIdAsync(id);
        public virtual async Task<TUser> GetUserByNameAsync(string userName) => await _userManager.FindByNameAsync(userName);
        public virtual async Task<TUser> GetUserByEmailAsync(string email) => await _userManager.FindByEmailAsync(email);
        public virtual async Task<IList<string>> GetUserRolesAsync(string id) => await _userManager.GetRolesAsync(await GetUserByIdAsync(id));
        public virtual async Task<IList<TUser>> GetUsersByRoleAsync(TRole role) => await _userManager.GetUsersInRoleAsync(role.Name);
        public virtual async Task<bool> CheckPasswordAsync(TUser user, string password) => await _userManager.CheckPasswordAsync(user, password);
        public virtual async Task<IdentityResult> ChangePassword(TUser user, string currentPwd, string newPwd) => await _userManager.ChangePasswordAsync(user, currentPwd, newPwd);
        public virtual async Task<SecurityToken> CreateAuthorizationToken(TUser user) => await BuildToken(user);
        public virtual async Task<IdentityResult> Remove(TUser user) => await _userManager.DeleteAsync(user);
        public virtual async Task<IdentityResult> Remove(string id) => await _userManager.DeleteAsync(await GetUserByIdAsync(id));
        public virtual async Task<IdentityResult> Update(TUser user) => await _userManager.UpdateAsync(user);
        public virtual async Task<IdentityResult> UpdateUserRoleAsync(string id, List<string> roles) => await _userManager.AddToRolesAsync(await GetUserByIdAsync(id), roles);
        private async Task<SecurityToken> BuildToken(TUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            JwtSecurityTokenHandler tokenHandler = new();
            List<Claim> claims = new()
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, user.RealName ?? ""),
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRange(roles.Select(r => new Claim("role", r)));
            SymmetricSecurityKey signingKey = new(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            };
            return tokenHandler.CreateToken(tokenDescriptor);
        }
    }
}
