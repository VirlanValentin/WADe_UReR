﻿using System.Web.Http;
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
        public UrerActionResult Get()
        {
            return placesRestClient.Get();
        }
    }
}