using System;
using System.Web.Http;
using MoviesLogic;

namespace MoviesMicroservice.Controllers
{
    public class MoviesController : ApiController
    {
        public MoviesController()
        {
            MoviesManager = new MoviesManager();
        }

        public MoviesManager MoviesManager { get; set; }

        //eg: GET api/movies?releaseDate={releaseDate}&genre={genre}
        [HttpGet]
        public IHttpActionResult Get([FromUri] DateTime releaseDate, [FromUri] string genre) //release date e null?
        {
            if (genre == null)
            {
                return BadRequest();
            }

            var result = MoviesManager.Get(releaseDate, genre);
            return Ok(result);
        }
    }
}
