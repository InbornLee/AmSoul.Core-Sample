namespace AmSoul.Core.Models
{
    public sealed class JwtTokenOptions
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public string SecurityKey { get; set; }
    }
}
