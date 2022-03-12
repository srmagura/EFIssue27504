namespace FunctionApp.ApiServices.Exceptions
{
    public class MissingRequestParameterException : Exception
    {
        public MissingRequestParameterException(string paramName)
            : base($"The request URL is missing a required parameter: {paramName}.") { }
    }
}
