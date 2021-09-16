namespace AmSoul.Core.Interfaces
{
    public interface IDatabaseSettings
    {
        string Host { get; set; }
        string Port { get; set; }
        string Username { get; set; }
        string Password { get; set; }
        string DatabaseName { get; set; }
        string ConnectionString { get; }
    }
}
