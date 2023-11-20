using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Models.ViewModels
{
    public class TemplateViewModel
    {
        public string? Key { get; set; }

        public string? Label { get; set; }

        public IEnumerable<TemplatePropertyModel>? RequiredProperties { get; set; }
    }

    public class MenuSubTypeViewModel
    {
        public string? Key { get; set; }

        public string? Label { get; set; }
    }
}
