using System;
using System.Web.Http;
using UrerGateway.Business.Models;
using UrerGateway.Business.RestClients;

namespace UrerGateway.Controllers
{
    public class PlacesController : ApiController
    {
        private PlacesRestClient placesRestClient;

        public PlacesController()
        {
            placesRestClient = new PlacesRestClient();
        }

        [HttpGet]
        public UrerActionResult Get([FromUri]double lat, [FromUri] double lon,
                                    [FromUri] double? radius=null, [FromUri] string type=null, [FromUri] int? limit=null, [FromUri] int? offset=null)
        {
            return placesRestClient.Get(lat, lon, radius, type, limit, offset);
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] string id)
        {
            return placesRestClient.Get(id);
        }

        [HttpGet]
        [Route("api/places/{id}/related")]
        public UrerActionResult GetRelatedPlaces([FromUri] string id, [FromUri] int? limit=null, [FromUri] int? offset=null)
        {
            return placesRestClient.GetRelatedPlaces(id, limit, offset);
        }

        [HttpGet]
        [Route("api/places/types")]
        public UrerActionResult GetTypes()
        {
            return placesRestClient.GetTypes();
        }
    }
}