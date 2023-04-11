using Management.Dashboard.Models;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface ITemplatesRepository
    {
        IEnumerable<TemplatePropertyModel> GetTemplateProperties(string layoutTemplateKey);
    }
}
