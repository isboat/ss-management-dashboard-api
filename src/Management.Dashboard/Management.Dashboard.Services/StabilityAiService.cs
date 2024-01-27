using Management.Dashboard.Services.Interfaces;
using Newtonsoft.Json;
using Management.Dashboard.Exceptions;
using System.Text;

namespace Management.Dashboard.Services
{
    public class StabilityAiService : IAiService
    {
        public StabilityAiService()
        {
        }

        public async Task<string?> GenerateAsync(string inputText, string tenantId)
        {
            var url = await ExecuteImagePrompt(inputText);
            return url;
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

        private static async Task<string?> DownloadImageAsync(string directoryPath, string fileName, byte[] dataByte)
        {
            // Create file path and ensure directory exists
            var path = Path.Combine(directoryPath, $"{fileName}.png");
            Directory.CreateDirectory(directoryPath);

            // Download the image and write to the file
            await File.WriteAllBytesAsync(path, dataByte);

            return path;
        }

        private static async Task<string?> ExecuteImagePrompt(string prompt)
        {
            using HttpClient client = new();
            using var req = new HttpRequestMessage(HttpMethod.Post, "URL_HERE");
            var apikey = "sk-mArvRyh4uXogRs7vblAjeDt4XtDgKlIdDANVVLViHeI8tgp5";

            var imageRequest = new ImageRequest 
            { 
                TextPrompts = new List<ImageTextPrompt>
                { 
                    new() { Text = prompt }
                }
            };

            req.Headers.Add("Authorization", $"Bearer {apikey}");
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
                    
                    var dataByte = Convert.FromBase64String(imageResponse.Artifacts.FirstOrDefault()?.Base64);
                    
                    return await DownloadImageAsync("images", "imageFile", dataByte);
                }
            }
            else
            {
                var dd = await response?.Content?.ReadAsStringAsync();
            }

            return null;
        }
    }

    public class ImageRequest
    {
        [JsonProperty("steps")]
        public int Steps { get; set; } = 40;


        [JsonProperty("width")]
        public int Width { get; set; } = 1024;


        [JsonProperty("height")]
        public int Height { get; set; } = 1024;


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
        public string Text { get; set; } = string.Empty;

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
        public string Base64 { get; set; }


        [JsonProperty("seed")]
        public string Seed { get; set; }


        [JsonProperty("finishReason")]
        public string FinishReason { get; set; }
    }
}
