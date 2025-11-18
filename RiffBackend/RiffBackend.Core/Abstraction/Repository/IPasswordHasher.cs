namespace RiffBackend.Core.Abstraction.Repository
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string password, string passwordHash);
    }
}