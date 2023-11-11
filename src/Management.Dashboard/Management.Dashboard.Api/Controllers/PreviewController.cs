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
    public class PreviewController : CustomBaseController
    {
        private readonly IPreviewService _previewService;

        public PreviewController(IPreviewService previewService)
        {
            _previewService = previewService;
        }

        [HttpGet("preview/{screenId}")]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Get(string screenId)
        {
            var tenantId = GetRequestTenantId();

            if (string.IsNullOrEmpty(tenantId))
            {
                return BadRequest();
            }

            var data = await _previewService.GetDataAsync(tenantId, screenId);
            if (data == null)
            {
                return NotFound();
            }

            return new JsonResult(data);
        }
    }
}
