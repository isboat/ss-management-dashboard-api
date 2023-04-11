namespace Management.Dashboard.Services.Interfaces
{
    public interface IUploadService
    {
        Task<bool> RemoveAsync(string tenantId, string fileName);
        Task<bool> UploadAsync(string tenantId, string fileName, Stream stream);
    }
}
