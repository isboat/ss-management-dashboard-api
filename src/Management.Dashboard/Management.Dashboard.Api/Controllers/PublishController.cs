using Amazon.Auth.AccessControlPolicy;
using Management.Dashboard.Common.Constants;
using Management.Dashboard.Models;
using Management.Dashboard.Models.History;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1/tenant/publish")]
    [ApiController]
    [Authorize(Policy = TenantAuthorization.RequiredPolicy)]
    public class PublishController : CustomBaseController
    {
        private readonly IPublishService _publishService;
        private readonly IHistoryService _historyService;

        public PublishController(IPublishService publishService, IHistoryService historyService)
        {
            _publishService = publishService;
            _historyService = historyService;
        }


        [HttpPost("screens/{id}")]
        public async Task<ActionResult> Post(string id)
        {
            var tenantId = GetRequestTenantId();
            if (string.IsNullOrEmpty(tenantId) || string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var result = await _publishService.PublishScreenAsync(tenantId, id);


            if (result)
            {
                string item = nameof(ScreenModel);
                await _historyService.StoreAsync(new HistoryModel
                {
                    ItemId = id,
                    ItemType = item,
                    Log = $"Published {item.Replace("Model","")}",
                    TenantId = tenantId,
                    User = GetAuthorizedUserInitials(),
                });
            }

            return result ? NoContent() : NotFound();
        }
    }
}
