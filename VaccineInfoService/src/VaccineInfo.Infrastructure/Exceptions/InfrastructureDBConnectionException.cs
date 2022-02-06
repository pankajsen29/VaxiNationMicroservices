using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VaccineInfo.Infrastructure.Exceptions
{
    public class InfrastructureDBConnectionException : InfrastructureLayerExceptionBase
    {
        public InfrastructureDBConnectionException(string message) : base(message)
        {
        }
    }
}
