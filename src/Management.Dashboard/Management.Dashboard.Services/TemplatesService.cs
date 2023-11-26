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
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuOverlay),
                    SubTypes = GetMenuSubTypes()
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MenuOnly,
                    Label = "Show Menu Only",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuOnly),
                    SubTypes = GetMenuSubTypes()
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MediaOnly,
                    Label = "Show Media Only",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MediaOnly)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MenuTopAndMediaBottom,
                    Label = "Menu top and Media bottom",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MenuTopAndMediaBottom),
                    SubTypes = GetMenuSubTypes()
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MediaTopAndMenuBottom,
                    Label = "Media top and menu bottom",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MediaTopAndMenuBottom),
                    SubTypes = GetMenuSubTypes()
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.Text,
                    Label = "Show Text",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.Text)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.DateTime,
                    Label = "Show Date and time",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.DateTime),
                    SubTypes = GetDateSubTypes()
                }
            };

            return templates;
        }

        private static IEnumerable<SubTypeViewModel> GetMenuSubTypes()
        {
            var list = new List<SubTypeViewModel>
            {
                new SubTypeViewModel
                {
                    Key = MenuSubTypeKeys.Basic,
                    Label = "Basic"
                },
                new SubTypeViewModel
                {
                    Key = MenuSubTypeKeys.Premium,
                    Label = "Premium"
                },
                new SubTypeViewModel
                {
                    Key = MenuSubTypeKeys.Deluxe,
                    Label = "Deluxe"
                }
            };

            return list;
        }

        private static IEnumerable<SubTypeViewModel> GetDateSubTypes()
        {
            var list = new List<SubTypeViewModel>
            {
                new SubTypeViewModel
                {
                    Key = DateTimeSubTypeKeys.British,
                    Label = "dd/mm/yyyy tt:mm:ss"
                },
                new SubTypeViewModel
                {
                    Key = DateTimeSubTypeKeys.American,
                    Label = "mm/dd/yyyy tt:mm:ss"
                }
            };

            return list;
        }
    }
}
