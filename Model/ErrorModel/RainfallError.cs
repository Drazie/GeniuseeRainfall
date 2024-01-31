using System;
namespace RainfallREST.Model.ErrorModel
{
	public class RainfallError
	{
        public string Message { get; set; }
        public List<RainfallErrorDetail> Details { get; set; }
    }
}

