using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Exceptions
{
    public class AiImageGenerationException: Exception
    {
        public AiImageGenerationException(string message) : base(message) { }
    }
}
