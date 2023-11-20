using Management.Dashboard.Models;
using Management.Dashboard.Models.ViewModels;
using Management.Dashboard.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Management.Dashboard.Api.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class TemplatesController : CustomBaseController
    {
        private readonly ITemplatesService _templatesService;

        public TemplatesController(ITemplatesService templatesService)
        {
            _templatesService = templatesService;
        }

        [HttpGet("templates")]
        [ProducesResponseType(200)]
        public IEnumerable<TemplateViewModel> Get()
        {
            return _templatesService.GetTemplates();
        }

        [HttpGet("menu-subtype-templates")]
        [ProducesResponseType(200)]
        public IEnumerable<MenuSubTypeViewModel> GetMenuTemplates()
        {
            return _templatesService.GetUIMenuSubTypes();
        }
    }
}
