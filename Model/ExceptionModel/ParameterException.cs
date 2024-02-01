namespace RainfallREST.Model.ExceptionModel
{
    public class ParameterException : BadHttpRequestException
    {
        public string ParameterName { get; private set; }
        public string DetailError { get; set; }


        public ParameterException(string message, string paramName, string detailError)
            : base(message)
        {
            ParameterName = paramName;
            DetailError = detailError;
        }
    }

}

