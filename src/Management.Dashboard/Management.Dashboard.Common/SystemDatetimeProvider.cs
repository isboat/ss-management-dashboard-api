using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Common
{
    public interface IDateTimeProvider
    {
        public DateTime? UtcNow { get; }

        public DateTime? UnixEpoch { get; }
    }

    public class SystemDatetimeProvider : IDateTimeProvider
    {
        public DateTime? UtcNow => DateTime.UtcNow;

        public DateTime? UnixEpoch => DateTime.UnixEpoch;
    }
}
