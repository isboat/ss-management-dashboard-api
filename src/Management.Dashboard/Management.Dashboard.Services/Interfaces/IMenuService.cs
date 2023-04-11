using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IMenuService
    {
        Task<IEnumerable<MenuModel>> GetMenusAsync(string tenantId);

        Task<MenuModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(MenuModel newModel);

        public Task UpdateAsync(string id, MenuModel updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
