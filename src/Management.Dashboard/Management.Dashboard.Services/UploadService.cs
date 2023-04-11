using Management.Dashboard.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Services
{
    public class UploadService : IUploadService
    {
        private readonly IContainerClientFactory _containerClientFactory;

        public UploadService(IContainerClientFactory containerClientFactory)
        {
            _containerClientFactory = containerClientFactory;
        }

        public async Task<bool> RemoveAsync(string tenantId, string fileName)
        {
            var container = _containerClientFactory.CreateAsync();
            var blob = container.GetBlobClient(CreatePath(tenantId, fileName));
            var result = await blob.DeleteIfExistsAsync(Azure.Storage.Blobs.Models.DeleteSnapshotsOption.IncludeSnapshots);
            return result;
        }

        public async Task<bool> UploadAsync(string tenantId, string fileName, Stream stream)
        {
            var container = _containerClientFactory.CreateAsync();
            var blob = container.GetBlobClient(CreatePath(tenantId, fileName));
            var result = await blob.UploadAsync(stream);

            return true;
        }

        private static string CreatePath(string tenantId, string filename)
        {
            return $"mediaasset/{tenantId}/{filename}";
        }
    }
}
