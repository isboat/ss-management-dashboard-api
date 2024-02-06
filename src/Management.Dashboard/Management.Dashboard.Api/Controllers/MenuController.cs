using Amazon.Auth.AccessControlPolicy;
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
    public class MenuController : CustomBaseController
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("menus")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(int? skip, int? limit)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _menuService.GetMenusAsync(tenantId, skip, limit);
            return data != null ? new JsonResult(data) : NotFound();
        }

        [HttpGet("menus/{id}")]
        [ProducesResponseType(typeof(MenuModel), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _menuService.GetAsync(tenantId, id);
            return data != null ? new JsonResult(data) : NotFound();
        }

        [HttpPost("menus")]
        public async Task<IActionResult> Post([FromBody] MenuModel model)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || model == null)
            {
                return BadRequest();
            }

            model.TenantId = tenantId;

            await _menuService.CreateAsync(model);
            return NoContent();
        }

        [HttpPatch("menus")]
        public async Task<IActionResult> Patch([FromBody] MenuModel model)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(model?.Id) || string.IsNullOrEmpty(model?.TenantId))
            {
                return BadRequest();
            }

            await _menuService.UpdateAsync(model.Id!, model);
            return NoContent();
        }

        [HttpDelete("menus/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _menuService.RemoveAsync(tenantId, id);
            return NoContent();
        }
    }
}
