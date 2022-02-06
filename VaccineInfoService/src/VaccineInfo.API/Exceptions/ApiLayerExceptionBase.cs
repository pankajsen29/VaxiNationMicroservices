namespace VaccineInfo.Api.Exceptions
{
    public abstract class ApiLayerExceptionBase : Exception
    {
        public ApiLayerExceptionBase(string message) : base(message)
        {
        }
    }
}
