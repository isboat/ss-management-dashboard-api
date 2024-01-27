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
    public class UserController : CustomBaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("users")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get()
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _userService.GetUsersAsync(tenantId);
            return data != null ? new JsonResult(data) : NotFound();
        }

        [HttpGet("users/{id}")]
        [ProducesResponseType(typeof(MenuModel), 200)]
        public async Task<IActionResult> Get(string id)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _userService.GetAsync(tenantId, id);
            return data != null ? new JsonResult(data) : NotFound();
        }

        [HttpPost("users")]
        public async Task<IActionResult> Post([FromBody] UserModel model)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || model == null)
            {
                return BadRequest();
            }

            model.TenantId = tenantId;

            try
            {
                await _userService.CreateAsync(model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return new ForbidResult(ex.Message);
            }
        }

        [HttpPatch("users")]
        public async Task<IActionResult> Patch([FromBody] UserModel model)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(model?.Id) || string.IsNullOrEmpty(model?.TenantId))
            {
                return BadRequest();
            }

            await _userService.UpdateAsync(model.Id!, model);
            return NoContent();
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            await _userService.RemoveAsync(tenantId, id);
            return NoContent();
        }
    }
}
