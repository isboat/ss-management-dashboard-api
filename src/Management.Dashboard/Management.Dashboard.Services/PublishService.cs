using Management.Dashboard.Common;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class PublishService : IPublishService
    {
        private readonly IPublishRepository _repository;
        private readonly IPreviewService _previewService;

        public PublishService(
            IPublishRepository repository, IPreviewService previewService)
        {
            _repository = repository;
            _previewService = previewService;
        }

        public async Task<bool> PublishDataAsync(string tenantId, string id)
        {
            var screenData = await _previewService.GetDataAsync(tenantId, id);
            if (screenData == null) return false;

            screenData.Checksum = MD5HashGenerator.GenerateKey(screenData);

            return await _repository.PublishScreenAsync(screenData);
        }

        public async Task<bool> ArchiveDataAsync(string tenantId, string id)
        {
            return await _repository.ArchiveScreenAsync(tenantId, id);
        }
    }
}