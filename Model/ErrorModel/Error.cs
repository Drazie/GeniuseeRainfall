namespace RainfallREST.Model.ErrorModel
{
    /// <summary>
    /// Represents an error response from the API.
    /// </summary>
    public class Error
	{
        /// <summary>
        /// A message that describes the error.
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Detailed information about the error.
        /// </summary>
        public List<ErrorDetail> Detail { get; set; } = new List<ErrorDetail>();
    }
}

