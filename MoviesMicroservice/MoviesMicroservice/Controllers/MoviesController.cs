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
        public IHttpActionResult Get([FromUri] string genre)
        {
            if (genre == null)
            {
                return BadRequest();
            }

            var result = MoviesManager.Get(genre);
            return Ok(result);
        }


        //eg: GET api/movies/id

        [Route("api/movies/{id}")]
        [HttpGet]
        public IHttpActionResult GetById([FromUri] Guid id) 
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var result = MoviesManager.GetById(id);


            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [Route("api/movies/{id}/related")]
        [HttpGet]
        public IHttpActionResult GetRelated([FromUri] Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }

            var result = MoviesManager.GetRelated(id);

            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult Get() 
        {
            return Ok("Ausdhiusahbfdisdfhsdhgf");
        }

        //eg: GET api/movies/genres
        [HttpGet]
        [Route("api/movies/genres")]
        public IHttpActionResult GetGenres()
        {
            var result = MoviesManager.GetGenres();
            return Ok(result);
        }

        //eg: GET api/movies/genres
        [HttpGet]
        [Route("api/movies/genres/{id}")]
        public IHttpActionResult GetGenresById([FromUri] Guid id)
        {
            var result = MoviesManager.GetGenreById(id);
            return Ok(result);
        }
    }
}
