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
        Task<IEnumerable<ScreenModel>> GetScreensAsync(string tenantId, int? skip, int? limit);

        Task<ScreenModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(ScreenModel newModel, string creator);

        public Task UpdateAsync(string id, ScreenModel updatedModel, string updator);

        public Task RemoveAsync(string tenantId, string id, string deletor);
    }
}
