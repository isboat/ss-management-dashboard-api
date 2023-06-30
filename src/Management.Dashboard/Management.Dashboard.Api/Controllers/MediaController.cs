using Amazon.Auth.AccessControlPolicy;
using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Services;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)]
    public class MediaController : CustomBaseController
    {
        private readonly IUploadService _uploadService;
        private readonly IAssetService _assetService;

        private const long UploadMaxSixe = 3_000_000_000;

        public MediaController(IUploadService uploadService, IAssetService assetService)
        {
            _uploadService = uploadService;
            _assetService = assetService;
        }

        [HttpGet("media-assets")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _assetService.GetAllAsync(tenantId);
            return data != null ? new JsonResult(data) : NotFound();
        }

        [HttpPost("media-asset/upload")]
        [RequestSizeLimit(UploadMaxSixe)]
        [RequestFormLimits(MultipartBodyLengthLimit = UploadMaxSixe)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> Upload([FromForm]MediaUploadModel model)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            if (model?.File == null)
            {
                return BadRequest("file is null");
            }

            var allowedFileExt = "image/jpeg,image/png,mp4";

            
            if (!allowedFileExt.Contains(model.File.ContentType))
            {
                return BadRequest($"{model.File.ContentType} Not allowed");
            }

            long size = model.File.Length;

            if (model.File.Length > 0)
            {
                await using var stream = model.File.OpenReadStream();
                var storagePath = await _uploadService.UploadAsync(tenantId, model.File.FileName.ToLowerInvariant(), stream);
                if (!string.IsNullOrEmpty(storagePath))
                {
                    await _assetService.CreateAsync(
                        new AssetItemModel
                        {
                            AssetUrl = storagePath,
                            Name = model.Title,
                            TenantId = tenantId,
                            Id = Guid.NewGuid().ToString("D"),
                            Description = model.Description,
                            Type = AssetType.Image
                        });
                }
            }

            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            return Ok(new { filename = model.File.FileName, size });
        }

        [HttpDelete("{tenantId}/media-asset/{filename}")]
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

    public class MediaUploadModel
    {
        public string Title { get; set; }

        public string Description { get; set; }
        public IFormFile File { get; set; }
    }
}
