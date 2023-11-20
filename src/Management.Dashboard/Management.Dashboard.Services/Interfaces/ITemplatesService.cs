using Management.Dashboard.Models;
using Management.Dashboard.Models.ViewModels;

namespace Management.Dashboard.Services.Interfaces
{
    public interface ITemplatesService
    {
        IEnumerable<TemplateViewModel> GetTemplates();

        IEnumerable<MenuSubTypeViewModel> GetUIMenuSubTypes();
    }
}
