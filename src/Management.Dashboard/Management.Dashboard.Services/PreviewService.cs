using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class PreviewService : IPreviewService
    {
        private readonly IRepository<ScreenModel> _repository;

        public PreviewService(IRepository<ScreenModel> repository)
        {
            _repository = repository;
        }

        Task<ScreenModel?> IPreviewService.GetDataAsync(string tenantId, string id)
        {
            throw new NotImplementedException();
        }
    }
}