using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineInfo.Core.Exceptions
{
    public abstract class CoreLayerExceptionBase : Exception
    {
        public CoreLayerExceptionBase(string message) : base(message)
        { 
        }
    }
}
