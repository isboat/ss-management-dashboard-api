namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IUserRepository<T> : IRepository<T>
    {
        Task<T?> GetByEmailPasswordAsync(string email, string password);
        Task<T?> GetByEmailAsync(string email);

        Task UpdatePasswordAsync(string tenantId, string id, string hashedPasswd);
    }
}
