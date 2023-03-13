using Management.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        Task<T?> GetAsync(string tenantId, string id);

        Task CreateAsync(T newModel);

        Task UpdateAsync(string id, T updatedModel);

        Task RemoveAsync(string tenantId, string id);
    }

    public interface IScreenRepository: IRepository<ScreenModel>
    {
        Task<List<ScreenModel>> GetScreensAsync(string tenantId);
    }
}
