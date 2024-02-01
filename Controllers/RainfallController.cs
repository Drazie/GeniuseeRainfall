using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RainfallREST.Model.ErrorModel;
using RainfallREST.Model.ExceptionModel;
using RainfallREST.Model.ResponseModel;

namespace RainfallREST.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RainfallController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public RainfallController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }
        //get the list of stations
        //https://environment.data.gov.uk/flood-monitoring/id/stations?parameter=rainfall

        /// <summary>
        /// Retrieve the latest rainfall readings for the specified station ID.
        /// </summary>
        /// <param name="stationId">The ID of the reading station. This is a path parameter that identifies the station.</param>
        /// <param name="count">The number of readings to return. This is an optional query parameter. If not specified, a default of 10 is used. The value must be between 1 and 100.</param>
        /// <returns>A list of rainfall readings for the given station ID. If no readings are found, a 404 error is returned.</returns>
        /// <response code="200">A list of rainfall readings successfully retrieved.</response>
        /// <response code="400">The request is invalid due to an incorrect station ID or count parameter.</response>
        /// <response code="404">No readings found for the specified stationId.</response>
        [HttpGet("id/{stationId}/readings")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RainfallReadingResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Error))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(Error))]
        public async Task<RainfallReadingResponse?> GetRainfallReadingsFromSpecificStation(string stationId, int count = 10)
        {
            if (count < 1 || count > 100)  //example of ErrorDetail
            {
                throw new ParameterException("Invalid request", nameof(count), "Out of range");
            }
            var rootUrl = "http://environment.data.gov.uk/flood-monitoring/id/stations";
            var requestLink = $"{rootUrl}/{stationId}/readings?_limit={count}";
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(requestLink);

            if (!response.IsSuccessStatusCode)
            {
                var message = response.StatusCode == System.Net.HttpStatusCode.NotFound
                    ? $"No readings found for the specified stationId: {stationId}"
                    : $"Error retrieving readings: {response.ReasonPhrase}";

                throw new HttpRequestException(message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var readings = JsonConvert.DeserializeObject<RainfallReadingResponse>(content);

            if (readings?.Items == null || !readings.Items.Any())
            {
                throw new KeyNotFoundException($"No readings found for the specified stationId: {stationId}");
            }

            return readings;
        }
    }
}

