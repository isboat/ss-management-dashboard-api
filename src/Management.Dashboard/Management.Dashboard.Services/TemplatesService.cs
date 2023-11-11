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
            var templates = new List<TemplateViewModel>
            {
                new TemplateViewModel
                {
                    Key = TemplateKeys.MenuOverlay,
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuOverlay)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MenuOnly,
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuOnly)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MediaOnly,
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MediaOnly)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MenuTopAndMediaBottom,
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuTopAndMediaBottom)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MediaTopAndMenuBottom,
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MediaTopAndMenuBottom)
                }
            };

            return templates;
        }
    }
}
