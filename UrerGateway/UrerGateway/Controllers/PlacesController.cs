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
            this.placesRestClient = new PlacesRestClient();
        }

        [HttpGet]
        public UrerActionResult Get()
        {
            return this.placesRestClient.Get();
        }
    }
}