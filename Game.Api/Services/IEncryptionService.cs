namespace Game.Api.Services
{
    public interface IEncryptionService
    {
        string CreateSaltKey(int size);
        string CreatePasswordHash(string password, string saltkey);
    }
}
