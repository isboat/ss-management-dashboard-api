using Management.Dashboard.Models.Encryption;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IEncryptionService
    {
        EncryptedResult? Encrypt(string input);

        bool Verify(string input, string storedHash);
    }
}
