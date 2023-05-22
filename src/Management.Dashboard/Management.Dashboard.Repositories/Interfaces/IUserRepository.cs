namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IUserRepository<T> : IRepository<T>
    {
        Task<T?> GetByEmailPasswordAsync(string email, string password);
    }
}
