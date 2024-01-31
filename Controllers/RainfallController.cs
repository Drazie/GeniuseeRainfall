using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RainfallREST.Model.ResponseModel;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        //to get the list of stations
        //https://environment.data.gov.uk/flood-monitoring/id/stations?parameter=rainfall



        [HttpGet("id/{stationId}/readings")]
        public async Task<IActionResult> GetRainfallReadingsFromSpecificStation(string stationId, int count = 10)
        {
            //http://environment.data.gov.uk/flood-monitoring
            var root = "http://environment.data.gov.uk/flood-monitoring/id/stations";
            var requestLink = $"{root}/{stationId}/readings?_limit={count}";
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync(requestLink);

            var content = await response.Content.ReadAsStringAsync();
            // Deserialize and map the response to your model
            var readings = JsonConvert.DeserializeObject<RainfallReadingResponse>(content);

            return Ok(readings);
        }
        
    }
}

