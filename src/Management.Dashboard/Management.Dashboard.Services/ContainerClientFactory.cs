using Azure.Storage.Blobs;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class ContainerClientFactory : IContainerClientFactory
    {
        public BlobContainerClient CreateAsync()
        {
            return new BlobContainerClient("", "");
        }
    }
}
