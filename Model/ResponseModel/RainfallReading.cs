namespace RainfallREST.Model.ResponseModel
{
    /// <summary>
    /// Details of a rainfall reading.
    /// </summary>
    public class RainfallReading
	{
        /// <summary>
        /// The date and time when the reading was measured.
        /// </summary>
        public DateTime DateTime { get; set; }
        /// <summary>
        /// The amount of rainfall measured.
        /// </summary>
        public decimal Value { get; set; }
    }
}

