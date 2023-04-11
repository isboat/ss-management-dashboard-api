using Management.Dashboard.Models;
using Management.Dashboard.Models.ViewModels;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class TemplatesService : ITemplatesService
    {
        private readonly ITemplatesRepository _templatesRepository;

        public TemplatesService(ITemplatesRepository templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        public IEnumerable<TemplateViewModel> GetTemplates()
        {
            var templates = new List<TemplateViewModel>();
            var menuBasic = new TemplateViewModel
            {
                Key = TemplateKeys.MenuBasic,
                RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuBasic)
            };

            templates.Add(menuBasic);


            var menuOverlay = new TemplateViewModel
            {
                Key = TemplateKeys.MenuOverlay,
                RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuOverlay)
            };
            templates.Add(menuOverlay);


            var a2 = new TemplateViewModel
            {
                Key = TemplateKeys.A2,
                RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.A2)
            };
            templates.Add(a2);


            var a3 = new TemplateViewModel
            {
                Key = TemplateKeys.A3,
                RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.A3)
            };
            templates.Add(a2);

            return templates;
        }
    }
}
