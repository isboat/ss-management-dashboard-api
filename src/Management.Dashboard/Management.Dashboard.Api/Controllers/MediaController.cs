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

        [HttpGet("media-assets/{id}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> GetById(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var data = await _assetService.GetAsync(tenantId, id);
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

            var allowedVideoFileExt = "video/mp4";
            var allowedImageFileExt = "image/jpeg,image/png";
            var storagePath = "";
            long size = 0;
            var fileName = "";

            bool isImageFile;
            if (model.IsAi)
            {
                var aiImagePath = await _aiService.GenerateAsync(model.Description, tenantId);

                if (aiImagePath == null) return BadRequest("ai_image_path_null");

                fileName = $"{model.Title.Replace(" ", "_")}.png";
                var file = System.IO.File.OpenRead(aiImagePath);
                storagePath = await _uploadService.UploadAsync(tenantId, fileName, file);
                isImageFile = true;
            }
            else
            {
                if (model?.File == null)
                {
                    return BadRequest("file is null");
                }
                isImageFile = allowedImageFileExt.Contains(model.File.ContentType);

                if (!isImageFile && !allowedVideoFileExt.Contains(model.File.ContentType))
                {
                    return BadRequest($"{model.File.ContentType} Not allowed");
                }

                size = model.File.Length;
                if (model.File.Length > 0)
                {
                    fileName = model.File.FileName.ToLowerInvariant();
                    await using var stream = model.File.OpenReadStream();
                    storagePath = await _uploadService.UploadAsync(tenantId, fileName, stream);
                }
            }

            if (!string.IsNullOrEmpty(storagePath))
            {
                var assetId = Guid.NewGuid().ToString("N");
                await _assetService.CreateAsync(
                    new AssetItemModel
                    {
                        AssetUrl = storagePath,
                        Name = model.Title,
                        TenantId = tenantId,
                        Id = assetId,
                        Description = model.Description,
                        Type = isImageFile ? AssetType.Image : AssetType.Video,
                        FileName = fileName
                    });

                // Process uploaded files
                // Don't rely on or trust the FileName property without validation.

                return Ok(new { filename = fileName, size, id = assetId });
            }

            return BadRequest();
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

            var dBAsset = await _assetService.GetAsync(tenantId, id);
            if (dBAsset == null)
            {
                return NotFound();
            }

            var deleteResult = await _uploadService.RemoveAsync(tenantId, dBAsset.FileName);
            if (!deleteResult) return BadRequest("unable_to_delete");

            await _assetService.RemoveAsync(tenantId, dBAsset.Id);
            return NoContent();
        }


        [HttpPatch("media-assets/{id}/name/{name}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> PatchName(string id, string name)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
            {
                return BadRequest();
            }

            var asset = await _assetService.GetAsync(tenantId, id);
            if (asset == null) return BadRequest();

            asset.Name = name;
            await _assetService.UpdateAsync(id, asset);

            return NoContent();
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


        [HttpDelete("media-assets/{id}/playlist/{playlistId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> RemoveMediaFromPlaylist(string id, string playlistId)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(playlistId))
            {
                return BadRequest();
            }

            await _playlistsService.RemoveMediaFromPlaylist(tenantId, playlistId, id);
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
