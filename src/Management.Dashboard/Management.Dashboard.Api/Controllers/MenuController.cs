using Management.Dashboard.Models;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("{tenantId}/menus")]
        [ProducesResponseType(200)]
        public async Task<IEnumerable<MenuModel>> Get(string tenantId)
        {
            return await _menuService.GetMenusAsync(tenantId);
        }

        [HttpGet("{tenantId}/menus/{id}")]
        [ProducesResponseType(typeof(MenuModel), 200)]
        public async Task<MenuModel?> Get(string tenantId, string id)
        {
            return await _menuService.GetAsync(tenantId, id);
        }

        [HttpPost("{tenantId}/menus")]
        public async Task<ActionResult> Post(string tenantId, [FromBody] MenuModel model)
        {
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(model?.TenantId))
            {
                return BadRequest();
            }

            await _menuService.CreateAsync(model);
            return NoContent();
        }

        [HttpPatch("{tenantId}/menus")]
        public async Task<ActionResult> Patch(string tenantId, [FromBody] MenuModel model)
        {
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(model?.Id) || string.IsNullOrEmpty(model?.TenantId))
            {
                return BadRequest();
            }

            await _menuService.UpdateAsync(model.Id!, model);
            return NoContent();
        }

        [HttpDelete("{tenantId}/menus/{id}")]
        public async Task<ActionResult> Delete(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _menuService.RemoveAsync(tenantId, id);
            return NoContent();
        }
    }
}
