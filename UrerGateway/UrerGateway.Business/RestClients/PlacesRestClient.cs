using System;
using System.Configuration;
using UrerGateway.Business.Models;

namespace UrerGateway.Business.RestClients
{
    public class PlacesRestClient : BaseRestClient
    {
        public PlacesRestClient()
        {
            var address = ConfigurationManager.AppSettings["PlacesLink"];
            BaseAddress = new Uri(address);
        }

        public UrerActionResult Get(double lat, double lon, double radius, string type, int limit, int offset)
        {
            var path = "Places-API/api/places?lat=" + lat
                        + "&lon=" + lon
                        + "&radius=" + radius
                        + "&type=" + type
                        + "&limit=" + limit
                        + "&offset=" + offset;
            return this.Get(path);
        }

        public UrerActionResult Get(DateTime releaseDate, string genre)
        {
            var path = "Movies?releaseDate=" + releaseDate + "&genre=" + genre;
            return base.Get(path);
        }

        public UrerActionResult Get(Guid id)
        {
            var path = "Places-API/api/places/" + id;
            return base.Get(path);
        }

        public UrerActionResult GetRelatedPlaces(Guid id, int limit, int offset)
        {
            var path = "Places-API/api/places/" + id + "/related?limit=" + limit + "&offset=" + offset;
            return base.Get(path);
        }
    }
}