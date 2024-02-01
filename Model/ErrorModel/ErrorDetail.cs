namespace RainfallREST.Model.ErrorModel
{
    /// <summary>
    /// Represents detailed information about an error related to a specific request property.
    /// </summary>
    public class ErrorDetail
	{
        /// <summary>
        /// The name of the property related to the error.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// A message that describes the error related to the property.
        /// </summary>
        public string Message { get; set; }
    }
}

