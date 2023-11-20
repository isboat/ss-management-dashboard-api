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
                    Label = "Menu Overlay",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuOverlay)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MenuOnly,
                    Label = "Menu Only",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuOnly)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MediaOnly,
                    Label = "Media Only",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MediaOnly)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MenuTopAndMediaBottom,
                    Label = "Menu top and Media bottom",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuTopAndMediaBottom)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MediaTopAndMenuBottom,
                    Label = "Media top and menu bottom",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MediaTopAndMenuBottom)
                }
            };

            return templates;
        }

        public IEnumerable<MenuSubTypeViewModel> GetUIMenuSubTypes()
        {
            var list = new List<MenuSubTypeViewModel>
            {
                new MenuSubTypeViewModel
                {
                    Key = MenuSubTypeKeys.Basic,
                    Label = "Basic"
                },
                new MenuSubTypeViewModel
                {
                    Key = MenuSubTypeKeys.Premium,
                    Label = "Premium"
                },
                new MenuSubTypeViewModel
                {
                    Key = MenuSubTypeKeys.Deluxe,
                    Label = "Deluxe"
                }
            };

            return list;
        }
    }
}
