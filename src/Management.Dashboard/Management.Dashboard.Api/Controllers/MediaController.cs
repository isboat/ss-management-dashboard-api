using Management.Dashboard.Models;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IScreenService _screenService;
        private readonly IUploadService _uploadService;

        private const long UploadMaxSixe = 3_000_000_000;

        public MediaController(IScreenService screenService, IUploadService uploadService)
        {
            _screenService = screenService;
            _uploadService = uploadService;
        }

        [HttpPost("{tenantId}/media/upload")]
        [RequestSizeLimit(UploadMaxSixe)]
        [RequestFormLimits(MultipartBodyLengthLimit = UploadMaxSixe)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Upload(string tenantId, List<IFormFile> files)
        {
            var allowedFileExt = "jpg,png,mp4";
            if (string.IsNullOrEmpty(tenantId) || files == null) return BadRequest();

            foreach (var file in files)
            {
                if (!allowedFileExt.Contains(file.ContentType))
                {
                    return BadRequest($"{file.ContentType} Not allowed");
                }
            }

            if (string.IsNullOrWhiteSpace(tenantId)) return BadRequest();

            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    await using var stream = formFile.OpenReadStream();
                    await _uploadService.UploadAsync(tenantId, formFile.FileName.ToLowerInvariant(), stream);
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { count = files.Count, size });
        }

        [HttpDelete("{tenantId}/media/{filename}")]
        public async Task<ActionResult> Delete(string tenantId, string filename)
        {
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(filename))
            {
                return BadRequest();
            }

            await _uploadService.RemoveAsync(tenantId, filename.ToLowerInvariant());
            return NoContent();
        }
    }
}
