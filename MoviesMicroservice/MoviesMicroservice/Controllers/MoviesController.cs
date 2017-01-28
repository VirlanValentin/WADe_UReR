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
        public IHttpActionResult Get([FromUri] DateTime releaseDate, [FromUri] string genre)
        {
            if (genre == null)
            {
                return BadRequest();
            }

            var result = MoviesManager.Get(releaseDate, genre);
            return Ok(result);
        }


        //eg: GET api/movies?&genre={genre}
        [HttpGet]
        public IHttpActionResult Get([FromUri] string genre) //release date e null?
        {
            if (genre == null)
            {
                return BadRequest();
            }

            var result = MoviesManager.Get(genre);
            return Ok(result);
        }



        [HttpGet]
        public IHttpActionResult Get() 
        {
            return Ok("Ausdhiusahbfdisdfhsdhgf");
        }
    }
}
