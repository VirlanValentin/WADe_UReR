using System;
using System.Configuration;
using UrerGateway.Business.Models;

namespace UrerGateway.Business.RestClients
{
    public class PlacesRestClient: BaseRestClient
    {
        public PlacesRestClient()
        {
            var address = ConfigurationManager.AppSettings["PlacesLink"];
            this.BaseAddress = new Uri(address);
        }

        public UrerActionResult Get()
        {
            var path = "Places-API/api/places";
            return this.Get(path);
        }
    }
}