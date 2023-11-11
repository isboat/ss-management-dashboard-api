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
    }
}
