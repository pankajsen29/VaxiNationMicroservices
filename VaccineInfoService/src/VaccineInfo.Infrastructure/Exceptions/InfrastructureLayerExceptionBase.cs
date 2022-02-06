using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineInfo.Infrastructure.Exceptions
{
    public abstract class InfrastructureLayerExceptionBase : Exception
    {
        public InfrastructureLayerExceptionBase(string message) : base(message)
        {
        }
    }
}
