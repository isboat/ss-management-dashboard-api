using Azure.Storage.Blobs;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IContainerClientFactory
    {
        BlobContainerClient CreateAsync();
    }
}
