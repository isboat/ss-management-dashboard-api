using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Repositories
{
    public class TemplatesRepository: ITemplatesRepository
    {
        public IEnumerable<TemplatePropertyModel> GetTemplateProperties(string layoutTemplateKey)
        {
            switch (layoutTemplateKey)
            {
                case TemplateKeys.MenuOverlay:
                    return MenuOverlayProperties();
                case TemplateKeys.MenuTopAndMediaBottom:
                    return MenuOverlayProperties();
                case TemplateKeys.MediaTopAndMenuBottom:
                    return MenuOverlayProperties();
                case TemplateKeys.MenuOnly:
                    return MenuOverlayProperties();
                case TemplateKeys.Text:
                    return TextProperties();
                case TemplateKeys.DateTime:
                    return TextProperties();
                case TemplateKeys.VideoStreaming:
                    return VideoStreamingProperties();
                default:
                    break;
            }

            return new List<TemplatePropertyModel>();  
        }

        private static IEnumerable<TemplatePropertyModel> MenuOverlayProperties()
        {
            return new List<TemplatePropertyModel> 
            { 
                new TemplatePropertyModel{ Key = "textColor", Label = "Text Color" },
                new TemplatePropertyModel { Key = "textFont", Label = "Text Font" },
                new TemplatePropertyModel { Key = "backgroundOpacity", Label = "Background Opacity" }
            };
        }

        private static IEnumerable<TemplatePropertyModel> TextProperties()
        {
            return new List<TemplatePropertyModel>
            {
                new() { Key = "textColor", Label = "Text Color" },
                new() { Key = "textFont", Label = "Text Font" },
                new() { Key = "text-align", Label = "Text Align" }
            };
        }

        private static IEnumerable<TemplatePropertyModel> VideoStreamingProperties()
        {
            return new List<TemplatePropertyModel>
            {
            };
        }
    }
}
