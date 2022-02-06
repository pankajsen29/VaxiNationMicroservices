using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineInfo.Core.Exceptions
{
    public class CoreNotFoundException : CoreLayerExceptionBase
    {
        public CoreNotFoundException(string message) : base(message)
        {
        }
    }
}
