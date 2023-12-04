using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IPlaylistsService
    {
        Task<IEnumerable<PlaylistModel>> GetAllAsync(string tenantId);

        Task<PlaylistModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(PlaylistModel newModel);

        public Task UpdateAsync(string id, PlaylistModel updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
