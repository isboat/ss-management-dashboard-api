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
                case TemplateKeys.A2:
                    return MenuOverlayProperties();
                case TemplateKeys.A3:
                    return MenuOverlayProperties();
                case TemplateKeys.MenuBasic:
                    return MenuOverlayProperties();
                default:
                    break;
            }

            throw new ArgumentOutOfRangeException();
        }

        private IEnumerable<TemplatePropertyModel> MenuOverlayProperties()
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
