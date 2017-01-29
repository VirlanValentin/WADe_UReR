using System;
using System.Configuration;
using UrerGateway.Business.Models;

namespace UrerGateway.Business.RestClients
{
    public class MoviesRestClient : BaseRestClient
    {
        public MoviesRestClient()
        {
            var address = ConfigurationManager.AppSettings["MoviesLink"];
            this.BaseAddress = new Uri(address);
        }

        public UrerActionResult Get()
        {
            var path = "Movies";
            return base.Get(path);
        }

        public UrerActionResult Get(Guid id)
        {
            var path = "Movies/" + id;
            return base.Get(path);
        }

        public UrerActionResult Get(string genre)
        {
            var path = "Movies?genre=" + genre;
            return base.Get(path);
        }

        public UrerActionResult Get(DateTime releaseDate, string genre)
        {
            var path = "Movies?releaseDate=" + releaseDate + "&genre=" + genre;
            return base.Get(path);
        }

        public UrerActionResult GetGenres()
        {
            var path = "Movies/Genres";
            return base.Get(path);
        }

        public UrerActionResult GetGenresById(Guid id)
        {
            var path = "Movies/Genres/" + id;
            return base.Get(path);
        }

        public UrerActionResult Post(object data)
        {
            var path = "Movies.Api/api/Movies/Post";
            return base.Post(path, data);
        }
    }
}