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
        private readonly IAiService _aiService;
        private readonly IPlaylistsService _playlistsService;

        private const long UploadMaxSixe = 3_000_000_000;

        public MediaController(
            IUploadService uploadService, 
            IAssetService assetService, 
            IAiService aiService, 
            IPlaylistsService playlistsService)
        {
            _uploadService = uploadService;
            _assetService = assetService;
            _aiService = aiService;
            _playlistsService = playlistsService;
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

        [HttpPost("media-assets")]
        [RequestSizeLimit(UploadMaxSixe)]
        [RequestFormLimits(MultipartBodyLengthLimit = UploadMaxSixe)]
        [ProducesResponseType(200)]
        public async Task<ActionResult> PostNew([FromForm]MediaUploadModel model)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var storagePath = "";
            long size = 0;
            var fileName = "";

            if (model.IsAi)
            {
                storagePath = await _aiService.GenerateAsync(model.Description, tenantId);
            }
            else
            {
                if (model?.File == null)
                {
                    return BadRequest("file is null");
                }

                var allowedFileExt = "image/jpeg,image/png,mp4";            
                if (!allowedFileExt.Contains(model.File.ContentType))
                {
                    return BadRequest($"{model.File.ContentType} Not allowed");
                }

                size = model.File.Length;

                if (model.File.Length > 0)
                {
                    await using var stream = model.File.OpenReadStream();
                    storagePath = await _uploadService.UploadAsync(tenantId, model.File.FileName.ToLowerInvariant(), stream);
                    fileName = model.File.FileName;
                }
            }

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

                // Process uploaded files
                // Don't rely on or trust the FileName property without validation.

                return Ok(new { filename = fileName, size });
            }

            return BadRequest();
        }


        [HttpPatch("media-assets/{id}/playlist/{playlistId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> AddMediaToPlaylist(string id, string playlistId)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(playlistId))
            {
                return BadRequest();
            }

            await _playlistsService.AddMediaToPlaylist(tenantId, playlistId, id);
            return NoContent();
        }

        [HttpDelete("media-assets/{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _uploadService.RemoveAsync(tenantId, id.ToLowerInvariant());
            return NoContent();
        }

    }

    public class MediaUploadModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsAi { get; set; }

        public IFormFile? File { get; set; }
    }
}
