namespace Management.Dashboard.Services.Interfaces
{
    public interface IUploadService
    {
        Task<bool> RemoveAsync(string tenantId, string fileName);
        Task<string> UploadAsync(string tenantId, string fileName, Stream stream);
    }
}
