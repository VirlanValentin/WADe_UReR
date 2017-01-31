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

        public UrerActionResult Get(double lat, double lon, double? radius, string type, int? limit, int? offset)
        {
            var path = "places?lat=" + lat.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture)
                       + "&lon=" + lon.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture);
            if (radius != null)
                path += "&radius=" + radius;

            if (type != null)
                path += "&type=" + type;


            if (limit != null)
                path += "&limit=" + limit;

            if (limit != null)
                path += "&offset=" + offset;

            return base.Get(path);
        }

        public new UrerActionResult Get(string id)
        {
            var path = "places/" + id;
            return base.Get(path);
        }

        public UrerActionResult GetRelatedPlaces(string id, int? limit, int? offset)
        {
            var path = "places/" + id + "/related";
            if (limit != null && offset != null)
            {
                path += "?limit=" + limit + "&offset=" + offset;
            }
            else
            {
                if (limit != null)
                {
                    path += "?limit=" + limit;
                }

                if (offset != null)
                {
                    path += "?offset=" + offset;
                }
            }
            return base.Get(path);
        }

        public UrerActionResult GetTypes()
        {
            const string path = "places/types";
            return base.Get(path);
        }
    }
}