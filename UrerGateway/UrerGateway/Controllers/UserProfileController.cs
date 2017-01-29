using System;
using System.Web.Http;
using UrerGateway.Business;
using UrerGateway.Business.Models;
using UrerGateway.Business.RestClients;

namespace UrerGateway.Controllers
{
    public class UserProfileController : ApiController
    {
        private readonly UserProfileRestClient userProfileRestClient;
        private readonly UserProfileManager userProfileManager;

        public UserProfileController()
        {
            this.userProfileRestClient = new UserProfileRestClient();
            this.userProfileManager = new UserProfileManager();
        }

        #region User

        [HttpGet]
        public UrerActionResult Get()
        {
            return this.userProfileRestClient.Get();
        }

        [HttpGet]
        public UrerActionResult GetByid([FromUri] Guid id)
        {
            return this.userProfileRestClient.Get(id);
        }

        [HttpPost]
        public UrerActionResult Register(object data)
        {
            return this.userProfileRestClient.Post(data);
        }

        #endregion

        #region Friends

        [HttpGet]
        [Route("api/UserProfile/{id}/friends")]
        public UrerActionResult GetFriends([FromUri] Guid id)
        {
            return this.userProfileRestClient.GetFriends(id);
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/friends/{friendId}")]
        public UrerActionResult AddFriend([FromUri] Guid id, [FromUri] Guid friendId)
        {
            return this.userProfileRestClient.AddFriend(id, friendId);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/friends/{friendId}")]
        public UrerActionResult RemoveFriend([FromUri] Guid id, [FromUri] Guid friendId)
        {
            return this.userProfileRestClient.RemoveFriend(id, friendId);
        }

        #endregion

        #region Enemies

        [HttpGet]
        [Route("api/UserProfile/{id}/enemies")]
        public UrerActionResult GetEnemy([FromUri] Guid id)
        {
            return this.userProfileRestClient.GetEnemies(id);
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/enemies/{enemyId}")]
        public UrerActionResult AddEnemy([FromUri] Guid id, [FromUri] Guid enemyId)
        {
            return this.userProfileRestClient.AddEnemy(id, enemyId);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/enemies/{enemyId}")]
        public UrerActionResult RemoveEnemy([FromUri] Guid id, [FromUri] Guid enemyId)
        {
            return this.userProfileRestClient.RemoveEnemy(id, enemyId);
        }

        #endregion

        #region Likes

        [HttpGet]
        [Route("api/UserProfile/{id}/likes/movies")]
        public UrerActionResult GetLikedMovies([FromUri] Guid id)
        {
            return this.userProfileManager.GetLikedMovies(id);
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/likes/movies/{movieId}")]
        public UrerActionResult AddMovie([FromUri] Guid id, [FromUri] Guid movieId)
        {
            return this.userProfileRestClient.AddMovie(id, movieId);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/likes/movies/{movieId}")]
        public UrerActionResult RemoveMovie([FromUri] Guid id, [FromUri] Guid movieId)
        {
            return this.userProfileRestClient.RemoveMovie(id, movieId);
        }

        [HttpGet]
        [Route("api/UserProfile/{id}/likes/places")]
        public UrerActionResult GetLikedPlaces([FromUri] Guid id)
        {
            return this.userProfileManager.GetLikedPlaces(id);
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/likes/places/{placesId}")]
        public UrerActionResult AddPlace([FromUri] Guid id, [FromUri] Guid placeId)
        {
            return this.userProfileRestClient.AddPlace(id, placeId);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/likes/places/{placeId}")]
        public UrerActionResult RemovePlace([FromUri] Guid id, [FromUri] Guid placeId)
        {
            return this.userProfileRestClient.RemovePlace(id, placeId);
        }

        #endregion

        #region Preferences

        [HttpGet]
        [Route("api/UserProfile/{id}/preferences/movies")]
        public UrerActionResult GetMoviePrefrences([FromUri] Guid id)
        {
            return this.userProfileManager.GetMoviePreferences(id);
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/preferences/movies/{movieId}")]
        public UrerActionResult AddMoviePreferences([FromUri] Guid id, [FromUri] Guid movieId)
        {
            return this.userProfileRestClient.AddMoviePreference(id, movieId);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/preferences/movies/{movieId}")]
        public UrerActionResult RemoveMoviePreferences([FromUri] Guid id, [FromUri] Guid movieId)
        {
            return this.userProfileRestClient.RemoveMoviePreferece(id, movieId);
        }

        [HttpGet]
        [Route("api/UserProfile/{id}/preferences/places")]
        public UrerActionResult GetPlacesPreferences([FromUri] Guid id)
        {
            return this.userProfileManager.GetPlacesPreferences(id);
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/preferences/places/{placesId}")]
        public UrerActionResult AddPlacePreferences([FromUri] Guid id, [FromUri] Guid placeId)
        {
            return this.userProfileRestClient.AddPlacePreference(id, placeId);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/preferences/places/{placeId}")]
        public UrerActionResult RemovePlacePreferences([FromUri] Guid id, [FromUri] Guid placeId)
        {
            return this.userProfileRestClient.RemovePlacePreference(id, placeId);
        }

        #endregion
    }
}