using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Common.Constants
{
    public class TenantAuthorization
    {
        public const string RequiredPolicy = "ManagementDashboard";
        public const string RequiredScope = "management.dashboard.content";


        public const string RequiredCorsPolicy = "allowSpecificOrigins";
    }
}
