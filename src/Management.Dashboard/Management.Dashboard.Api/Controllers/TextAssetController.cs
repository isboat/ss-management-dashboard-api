using Management.Dashboard.Api.ViewModels;
using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)]
    public class TextAssetController : CustomBaseController
    {
        private readonly ITextAssetService _assetService;
        private readonly IPlaylistsService _playlistsService;

        public TextAssetController(
            ITextAssetService assetService,
            IPlaylistsService playlistsService)
        {
            _assetService = assetService;
            _playlistsService = playlistsService;
        }

        [HttpGet("text-assets")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(int? skip, int? limit)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _assetService.GetAllAsync(tenantId, skip, limit);
            return data != null ? new JsonResult(data) : NotFound();
        }

        [HttpGet("text-assets/{id}")]
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

        [HttpPost("text-assets")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> PostNew(TextAssetCreationModel model)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }
            var createdId = await _assetService.CreateAsync(
                new TextAssetItemModel
                {
                    TenantId = tenantId,
                    Name = model.Title,
                    Description = model.Description,
                    BackgroundColor = model.BackgroundColor,
                    TextColor = model.TextColor
                });

            return new JsonResult(new { Id = createdId });
        }

        [HttpDelete("text-assets/{id}")]
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

            await _assetService.RemoveAsync(tenantId, dBAsset.Id!);
            return NoContent();
        }

        [HttpPatch("text-assets/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> PatchModel(TextAssetItemModel model)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(model.Id))
            {
                return BadRequest();
            }

            var asset = await _assetService.GetAsync(tenantId, model.Id);
            if (asset == null) return BadRequest();

            asset.Name = model.Name;
            asset.Description = model.Description;
            asset.BackgroundColor = model.BackgroundColor;
            asset.TextColor = model.TextColor;
            await _assetService.UpdateAsync(model.Id, asset);

            return NoContent();
        }


        [HttpPatch("text-assets/{id}/playlist/{playlistId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> AddToPlaylist(string id, string playlistId)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(playlistId))
            {
                return BadRequest();
            }

            await _playlistsService.AddToPlaylist(tenantId, playlistId, id, PlaylistItemType.Text);
            return NoContent();
        }


        [HttpDelete("text-assets/{id}/playlist/{playlistId}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult> RemoveFromPlaylist(string id, string playlistId)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id) || string.IsNullOrEmpty(playlistId))
            {
                return BadRequest();
            }

            await _playlistsService.RemoveFromPlaylist(tenantId, playlistId, id);
            return NoContent();
        }
    }
}
