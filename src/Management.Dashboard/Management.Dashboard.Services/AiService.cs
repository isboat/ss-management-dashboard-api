using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class AiService : IAiService
    {
        public AiService()
        {
        }

        public async Task<string> GenerateAsync(string inputText, string tenantId)
        {
            // generatedUrl = Ai.generate(inputtext)

            var folder = "images";
            var fileName = "test";
            var url = "https://cdn.discordapp.com/attachments/458291463663386646/592779619212460054/Screenshot_20190624-201411.jpg?query&with.dots";

            return await DownloadImageAsync(folder, fileName, new Uri(url));
        }

        private async Task<string> DownloadImageAsync(string directoryPath, string fileName, Uri uri)
        {
            using var httpClient = new HttpClient();

            // Get the file extension
            var uriWithoutQuery = uri.GetLeftPart(UriPartial.Path);
            var fileExtension = Path.GetExtension(uriWithoutQuery);

            // Create file path and ensure directory exists
            var path = Path.Combine(directoryPath, $"{fileName}{fileExtension}");
            Directory.CreateDirectory(directoryPath);

            // Download the image and write to the file
            var imageBytes = await httpClient.GetByteArrayAsync(uri);
            await File.WriteAllBytesAsync(path, imageBytes);

            return path;
        }
    }
}
