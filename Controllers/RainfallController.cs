using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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

        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadingsFromSpecificStation(string stationId, int count = 10)
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
                return NotFound(new { Message = $"No readings found for the specified stationId: {stationId}" });
            }

            return Ok(readings);
        }
    }
}

