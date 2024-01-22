using Management.Dashboard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Services.Extensions
{
    internal static class ModelItemExtensions
    {
        internal static void AddId(this IModelItem newModel)
        {
            if (string.IsNullOrEmpty(newModel.Id))
            {
                newModel.Id = Guid.NewGuid().ToString("N");
            }
        }
    }
}
