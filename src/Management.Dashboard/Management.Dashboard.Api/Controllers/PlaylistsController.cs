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
    public class PlaylistsController : CustomBaseController
    {
        private readonly IPlaylistsService _playlistsService;

        public PlaylistsController(IPlaylistsService playlistsService)
        {
            _playlistsService = playlistsService;
        }

        [HttpGet("playlists")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(int? skip, int? limit)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _playlistsService.GetAllAsync(tenantId, skip, limit);
            if (data == null)
            {
                return NotFound();
            }

            return new JsonResult(data);
        }

        [HttpGet("playlists/{id}")]
        [ProducesResponseType(typeof(PlaylistWithItemModel), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _playlistsService.GetWithItemsAsync(tenantId, id);
            return data != null ? new JsonResult(data) : NotFound();
        }


        [HttpPost("playlists")]
        public async Task<ActionResult> Post([FromBody] PlaylistModel model)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            model.TenantId = tenantId;

            await _playlistsService.CreateAsync(model);

            return NoContent();
        }


        [HttpPatch("playlists/{id}")]
        public async Task<ActionResult> PatchName(string id, [FromBody] PlaylistModel model)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(model?.Id))
            {
                return BadRequest();
            }

            model.TenantId = tenantId;

            await _playlistsService.UpdateAsync(model.Id, model);

            return NoContent();
        }

        [HttpDelete("playlists/{id}")]
        [ProducesResponseType(typeof(DeviceModel), 200)]
        public async Task<IActionResult> Delete(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _playlistsService.RemoveAsync(tenantId, id);
            return Ok();
        }
    }
}
