using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineInfo.Core.Exceptions
{
    public class CoreValidationException : CoreLayerExceptionBase
    {
        public CoreValidationException(string message) : base(message)
        {
        }
    }
}
