using Management.Dashboard.Common;
using Management.Dashboard.Models.History;
using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class PublishService : IPublishService
    {
        private readonly IPublishRepository _repository;
        private readonly IPreviewService _previewService;
        private readonly IHistoryService _historyService;

        public PublishService(
            IPublishRepository repository, IPreviewService previewService, IHistoryService historyService)
        {
            _repository = repository;
            _previewService = previewService;
            _historyService = historyService;
        }

        public async Task<bool> PublishScreenAsync(string tenantId, string id, string user)
        {
            var screenData = await _previewService.GetDataAsync(tenantId, id);
            if (screenData == null) return false;

            screenData.Checksum = MD5HashGenerator.GenerateKey(screenData);

            var result = await _repository.PublishScreenAsync(screenData);

            if (result)
            {
                string item = nameof(ScreenModel);
                await _historyService.StoreAsync(new HistoryModel
                {
                    ItemId = id,
                    ItemType = item,
                    Log = $"Published {screenData.DisplayName}",
                    TenantId = tenantId,
                    User = user,
                });
            }
            return result;
        }

        public async Task<bool> ArchiveDataAsync(string tenantId, string id, string user)
        {
            var result = await _repository.ArchiveScreenAsync(tenantId, id);

            if (result)
            {
                string item = nameof(ScreenModel);
                await _historyService.StoreAsync(new HistoryModel
                {
                    ItemId = id,
                    ItemType = item,
                    Log = $"Archived {item.Replace("Model", "")}",
                    TenantId = tenantId,
                    User = user,
                });
            }
            return result;
        }
    }
}