using Management.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IScreenService
    {
        Task<IEnumerable<ScreenModel>> GetScreensAsync(string tenantId);

        Task<ScreenModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(ScreenModel newModel);

        public Task UpdateAsync(string id, ScreenModel updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
