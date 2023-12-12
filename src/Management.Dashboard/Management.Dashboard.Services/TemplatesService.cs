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
                    Key = TemplateKeys.Text,
                    Label = "Show Text/Information",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.Text)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.DateTime,
                    Label = "Show Date and time",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.DateTime),
                    SubTypes = GetDateSubTypes()
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.MediaPlaylist,
                    Label = "Show Media Playlist",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.MediaPlaylist)
                },

                new TemplateViewModel
                {
                    Key = TemplateKeys.Weather,
                    Label = "Show current weather and forecast",
                    RequiredProperties = _templatesRepository.GetTemplateProperties(TemplateKeys.Weather)
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
                },
                new SubTypeViewModel
                {
                    Key = DateTimeSubTypeKeys.TimeFirstAmerican,
                    Label = "tt:mm:ss january 1, yyyy"
                },
                new SubTypeViewModel
                {
                    Key = DateTimeSubTypeKeys.TimeFirstBritish,
                    Label = "tt:mm:ss 1 january, yyyy"
                }
            };

            return list;
        }
    }
}
