using System;
using System.Web.Http;
using UrerGateway.Business.Models;
using UrerGateway.Business.RestClients;

namespace UrerGateway.Controllers
{
    public class MoviesController : ApiController
    {
        private readonly MoviesRestClient moviesRestClient;

        public MoviesController()
        {
            this.moviesRestClient = new MoviesRestClient();
        }

        [HttpGet]
        public UrerActionResult Get()
        {
            return this.moviesRestClient.Get();
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] Guid id)
        {
            return this.moviesRestClient.Get(id);
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] string genre)
        {
            return this.moviesRestClient.Get(genre);
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] DateTime releaseDate, [FromUri] string genre)
        {
            return this.moviesRestClient.Get(releaseDate, genre);
        }

        [HttpPost]
        public UrerActionResult Post(object data)
        {
            return this.moviesRestClient.Post(data);
        }

        [HttpGet]
        [Route("api/movies/genres")]
        public UrerActionResult GetGenres()
        {
            return this.moviesRestClient.GetGenres();
        }

        [HttpGet]
        [Route("api/movies/genres/{id}")]
        public UrerActionResult GetGenresById([FromUri] Guid id)
        {
            return this.moviesRestClient.GetGenresById(id);
        }
    }
}