using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class LocalUploadService : IUploadService
    {
        public LocalUploadService()
        {
        }

        public async Task<bool> RemoveAsync(string tenantId, string fileName)
        {
            return true;
        }

        public async Task<string> UploadAsync(string tenantId, string fileName, Stream stream)
        {
            var uploads = $"{CreatePath(tenantId)}\\uploads";
            if(!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var filePath = Path.Combine(uploads,fileName);
            stream.CopyTo(new FileStream(filePath, FileMode.Create));  

            return filePath;
        }

        private static string CreatePath(string tenantId)
        {
            return $"mediaasset\\{tenantId}";
        }
    }
}
