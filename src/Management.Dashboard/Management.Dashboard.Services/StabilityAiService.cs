using Management.Dashboard.Services.Interfaces;
using Newtonsoft.Json;
using Management.Dashboard.Exceptions;
using System.Text;
using Management.Dashboard.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace Management.Dashboard.Services
{
    public class StabilityAiService : IAiService
    {
        private readonly StabilityAiSettings _settings;
        private readonly ILogger<StabilityAiService> _logger;

        public StabilityAiService(IOptions<StabilityAiSettings> settings, ILogger<StabilityAiService> logger)
        {
            _settings = settings.Value;
            _logger = logger;
        }

        public async Task<string?> GenerateAsync(string inputText, string tenantId)
        {
            var url = await ExecuteImagePrompt(inputText, tenantId);
            return url;
        }

        private static async Task<string?> DownloadImageAsync(string directoryPath, string fileName, byte[] dataByte)
        {
            // Create file path and ensure directory exists
            var path = Path.Combine(directoryPath, $"{fileName}.png");
            Directory.CreateDirectory(directoryPath);

            // Download the image and write to the file
            await File.WriteAllBytesAsync(path, dataByte);

            return path;
        }

        private async Task<string?> ExecuteImagePrompt(string prompt, string tenantId)
        {
            try
            {

                using HttpClient client = new();
                using var req = new HttpRequestMessage(HttpMethod.Post, _settings.ApiUrl);

                var imageRequest = new ImageRequest
                {
                    TextPrompts = new List<ImageTextPrompt>
                {
                    new() { Text = prompt }
                }
                };

                req.Headers.Add("Authorization", $"Bearer {_settings.Apikey}");
                req.Headers.Add("Accept", "application/json");

                var reqContent = JsonConvert.SerializeObject(imageRequest);
                req.Content = new StringContent(reqContent, Encoding.UTF8, "application/json");

                using HttpResponseMessage? response = await client.SendAsync(req);
                if (response != null && response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseString))
                    {
                        var imageResponse = JsonConvert.DeserializeObject<ImageResponse>(responseString);

                        if (imageResponse?.Artifacts == null || !imageResponse.Artifacts.Any()) throw new AiImageGenerationException("AI image response is null");
                        var artifact = imageResponse.Artifacts.FirstOrDefault() ?? throw new AiImageGenerationException("AI image response is null");

                        var dataByte = Convert.FromBase64String(artifact.Base64!);

                        return await DownloadImageAsync(GetTenantIdTmpDirectoryPath(tenantId), GetTenantIdTmpFilePath(prompt), dataByte);
                    }
                }
                else
                {
                    _logger.LogError("Response is not successful. {Response}", JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occurred {ex}", ex);
            }

            return null;
        }

        private static string GetTenantIdTmpFilePath(string prompt)
        {
            var tmpName = prompt ?? "tmpfilename";
            tmpName = tmpName.ToLowerInvariant().Replace(" ","").Substring(0, 15);

            return tmpName;
        }

        private static string GetTenantIdTmpDirectoryPath(string tenantId)
        {
            return $"images-{tenantId}";
        }
    }

    public class ImageRequest
    {
        [JsonProperty("steps")]
        public int Steps { get; set; } = 40;


        [JsonProperty("width")]
        public int Width { get; set; } = 1216; //1024;


        [JsonProperty("height")]
        public int Height { get; set; } = 832; // 1024;


        [JsonProperty("seed")]
        public int Seed { get; set; } = 0;


        [JsonProperty("cfg_scale")]
        public int CfgScale { get; set; } = 5;


        [JsonProperty("samples")]
        public int Samples { get; set; } = 1;

        [JsonProperty("text_prompts")]
        public List<ImageTextPrompt> TextPrompts { get; set; } = new List<ImageTextPrompt>();
    }

    public class ImageTextPrompt
    {
        [JsonProperty("text")]
        public string? Text { get; set; } = string.Empty;

        [JsonProperty("weight")]
        public int Weight { get; set; } = 1;
    }

    public class ImageResponse
    {

        [JsonProperty("artifacts")]
        public ImageData[]? Artifacts { get; set; }
    }

    public class ImageData
    {

        [JsonProperty("base64")]
        public string? Base64 { get; set; }


        [JsonProperty("seed")]
        public string? Seed { get; set; }


        [JsonProperty("finishReason")]
        public string? FinishReason { get; set; }
    }
}
