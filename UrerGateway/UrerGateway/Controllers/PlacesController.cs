using System;
using System.Web.Http;
using UrerGateway.Business.Models;
using UrerGateway.Business.RestClients;

namespace UrerGateway.Controllers
{
    public class PlacesController
    {
        private PlacesRestClient placesRestClient;

        public PlacesController()
        {
            placesRestClient = new PlacesRestClient();
        }

        [HttpGet]
        public UrerActionResult Get([FromUri]double lat, [FromUri] double lon, [FromUri] double radius, [FromUri] string type, [FromUri] int limit, [FromUri] int offset)
        {
            return placesRestClient.Get(lat, lon, radius, type, limit, offset);
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] Guid id)
        {
            return placesRestClient.Get(id);
        }

        [HttpGet]
        [Route("api/places/{place_id}/related")]
        public UrerActionResult GetRelatedPlaces([FromUri] Guid id, [FromUri] int limit, [FromUri] int offset)
        {
            return placesRestClient.GetRelatedPlaces(id, limit, offset);
        }
    }
}