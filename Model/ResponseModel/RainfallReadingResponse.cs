namespace RainfallREST.Model.ResponseModel
{
    /// <summary>
    /// Details of a rainfall reading response.
    /// </summary>
    public class RainfallReadingResponse
	{
        /// <summary>
        /// A list of rainfall readings.
        /// </summary>
        public List<RainfallReading> Items { get; set; }
	}
}

