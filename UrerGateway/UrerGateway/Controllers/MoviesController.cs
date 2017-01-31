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
            moviesRestClient = new MoviesRestClient();
        }

        [HttpGet]
        public UrerActionResult Get()
        {
            return moviesRestClient.Get();
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] Guid id)
        {
            return moviesRestClient.Get(id);
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] string genre)
        {
            return moviesRestClient.Get(genre);
        }

        [HttpGet]
        public UrerActionResult Get([FromUri] DateTime releaseDate, [FromUri] string genre)
        {
            return moviesRestClient.Get(releaseDate, genre);
        }

        [HttpPost]
        public UrerActionResult Post(object data)
        {
            return moviesRestClient.Post(data);
        }

        [HttpGet]
        [Route("api/movies/genres")]
        public UrerActionResult GetGenres()
        {
            return moviesRestClient.GetGenres();
        }

        [HttpGet]
        [Route("api/movies/genres/{id}")]
        public UrerActionResult GetGenresById([FromUri] Guid id)
        {
            return moviesRestClient.GetGenresById(id);
        }

        [Route("api/movies/{id}/related")]
        [HttpGet]
        public IHttpActionResult GetRelated([FromUri] Guid id)
        {
            return moviesRestClient.GetRelated(id);
        }
    }
}