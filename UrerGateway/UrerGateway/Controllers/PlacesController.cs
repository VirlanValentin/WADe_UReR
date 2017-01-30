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
        public UrerActionResult Get(double lat, double lon, double radius, string type, int limit, int offset)
        {
            return placesRestClient.Get(lat, lon, radius, type, limit, offset);
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] Guid id)
        {
            return placesRestClient.Get(id);
        }
    }
}