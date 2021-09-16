using Microsoft.AspNetCore.Identity;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AmSoul.Core.Models
{
    public class BaseRole : Role<ObjectId>
    {
        public BaseRole() : base() { }

        public BaseRole(string name) : base(name) { }

    }
    public class Role<TKey> : IdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        public Role()
        {
            Claims = new List<IdentityRoleClaim<string>>();
        }
        public Role(string name) : this()
        {
            Name = name;
            NormalizedName = name.ToUpperInvariant();
        }
        public override string ToString()
        {
            return Name;
        }
        public List<IdentityRoleClaim<string>> Claims { get; set; }
        public string CharacterName { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
    }
    public sealed class RoleDto
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string CharacterName { get; set; }
        public string Description { get; set; }
        public string Tag { get; set; }
    }
}