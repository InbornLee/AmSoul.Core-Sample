using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmSoul.Core.Models
{
    public class BaseUser : BaseUser<ObjectId>
    {
        public BaseUser() : base() { }
        public BaseUser(string userName) : base(userName) { }
    }
    public class BaseUser<TKey> : IdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        public BaseUser()
        {
            Roles = new List<string>();
            Claims = new List<IdentityUserClaim<string>>();
            Logins = new List<IdentityUserLogin<string>>();
            Tokens = new List<IdentityUserToken<string>>();
            RecoveryCodes = new List<TwoFactorRecoveryCode>();
        }
        public BaseUser(string userName) : this()
        {
            UserName = userName;
            NormalizedUserName = userName.ToUpperInvariant();
        }
        public string RealName { get; set; }
        public byte[] HeaderImage { get; set; }
        public List<string> Roles { get; set; }

        public List<IdentityUserClaim<string>> Claims { get; set; }

        public List<IdentityUserLogin<string>> Logins { get; set; }

        public List<IdentityUserToken<string>> Tokens { get; set; }

        public List<TwoFactorRecoveryCode> RecoveryCodes { get; set; }
        [Obsolete("This property moved to Tokens and should not be used anymore! Will be removed in future versions.")]
        public string AuthenticatorKey { get; set; }


    }
    public class TwoFactorRecoveryCode
    {
        public string Code { get; set; }

        public bool Redeemed { get; set; }
    }
    public class UserDto
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        public string RealName { get; set; }
        public byte[] HeaderImage { get; set; }
        public List<string> Roles { get; set; }

    }
    public class ChangePasswordDto
    {
        [Required]
        [DataType(DataType.Password)]
        public string CurrentPwd { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPwd { get; set; }
    }
}
