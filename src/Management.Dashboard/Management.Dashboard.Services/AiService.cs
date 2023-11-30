using DnsClient.Internal;
using Management.Dashboard.Services.Interfaces;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

namespace Management.Dashboard.Services
{
    public class AiService : IAiService
    {
        public AiService()
        {
        }

        public async Task<string?> GenerateAsync(string inputText, string tenantId)
        {
            var folder = "images";
            var fileName = "test";
            var url = await ExecuteImagePrompt(inputText);
            if (string.IsNullOrEmpty(url)) return null;

            return await DownloadImageAsync(folder, fileName, new Uri(url));
        }

        private static async Task<string?> DownloadImageAsync(string directoryPath, string fileName, Uri uri)
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

        private static async Task<string?> ExecuteImagePrompt(string prompt)
        {
            using HttpClient client = new();
            using var req = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/images/generations");
            var apikey = "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";

            req.Headers.Add("Authorization", $"Bearer {apikey}");
            var reqContent = JsonConvert.SerializeObject(new ImageRequest { Prompt = prompt });
            req.Content = new StringContent(reqContent, Encoding.UTF8, "application/json");

            using HttpResponseMessage? response = await client.SendAsync(req);
            if (response != null && response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseString))
                {
                    var imageResponse = JsonConvert.DeserializeObject<ImageResponse>(responseString);
                    return imageResponse?.Data?.FirstOrDefault()?.Url;
                }
            }

            return null;
        }
    }

    public class ImageRequest
    {
        [JsonProperty("n")]
        public int NumberOfImages { get; set; } = 1;


        [JsonProperty("prompt")]
        public string? Prompt { get; set; }


        [JsonProperty("model")]
        public string? Model { get; set; } = "image-alpha-001";


        [JsonProperty("quality")]
        public string? Quality { get; set; } = "standard";


        [JsonProperty("size")]
        public string Size { get; set; } = "512x512";
    }

    public class ImageResponse
    {

        [JsonProperty("created")]
        public long? Created { get; set; }


        [JsonProperty("data")]
        public ImageData[]? Data { get; set; }
    }

    public class ImageData
    {

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
