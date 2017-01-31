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
            userProfileRestClient = new UserProfileRestClient();
            userProfileManager = new UserProfileManager();
        }

        #region User

        [HttpGet]
        public UrerActionResult Get()
        {
            return userProfileRestClient.Get();
        }

        [HttpGet]
        public UrerActionResult GetByid([FromUri] Guid id)
        {
            return userProfileRestClient.Get(id);
        }

        [HttpPost]
        [Route("api/UserProfile/GenerateQR")]
        public IHttpActionResult GetQrCode(object data)
        {
            return userProfileRestClient.GetQrCode(data);
        }

        [HttpPost]
        [Route("api/UserProfile/Register")]
        public UrerActionResult Register(object data)
        {
            return userProfileRestClient.Register(data);
        }

        [HttpPost]
        [Route("api/UserProfile/Login")]
        public UrerActionResult Login(object data)
        {
            return userProfileRestClient.Login(data);
        }

        [HttpPost]
        [Route("api/UserProfile/Logout")]
        public UrerActionResult Logout([FromBody] object data)
        {
            return userProfileRestClient.Logout(data);
        }

        #endregion

        #region Friends

        [HttpGet]
        [Route("api/UserProfile/{id}/friends")]
        public UrerActionResult GetFriends([FromUri] Guid id)
        {
            return userProfileRestClient.GetFriends(id);
        }

        [HttpPost]
        [Route("api/UserProfile/{id}/friends")]
        public UrerActionResult AddFriend([FromUri] Guid id, [FromBody] object data)
        {
            return userProfileRestClient.AddFriend(id, data);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/friends/{friendId}")]
        public UrerActionResult RemoveFriend([FromUri] Guid id, [FromUri] Guid friendId)
        {
            return userProfileRestClient.RemoveFriend(id, friendId);
        }

        #endregion

        #region Enemies

        [HttpGet]
        [Route("api/UserProfile/{id}/enemies")]
        public UrerActionResult GetEnemy([FromUri] Guid id)
        {
            return userProfileRestClient.GetEnemies(id);
        }

        [HttpPost]
        [Route("api/UserProfile/{id}/enemies")]
        public UrerActionResult AddEnemy([FromUri] Guid id, [FromBody] object data)
        {
            return userProfileRestClient.AddEnemy(id, data);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/enemies/{enemyId}")]
        public UrerActionResult RemoveEnemy([FromUri] Guid id, [FromUri] Guid enemyId)
        {
            return userProfileRestClient.RemoveEnemy(id, enemyId);
        }

        #endregion

        #region Likes

        [HttpGet]
        [Route("api/UserProfile/{id}/likes/movies")]
        public UrerActionResult GetLikedMovies([FromUri] Guid id)
        {
            return userProfileManager.GetLikedMovies(id);
        }

        [HttpPost]
        [Route("api/UserProfile/{id}/likes/movies")]
        public UrerActionResult AddMovie([FromUri] Guid id, [FromBody] object data)
        {
            return userProfileRestClient.AddMovie(id, data);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/likes/movies/{movieId}")]
        public UrerActionResult RemoveMovie([FromUri] Guid id, [FromUri] Guid movieId)
        {
            return userProfileRestClient.RemoveMovie(id, movieId);
        }

        [HttpGet]
        [Route("api/UserProfile/{id}/likes/places")]
        public UrerActionResult GetLikedPlaces([FromUri] Guid id)
        {
            return userProfileManager.GetLikedPlaces(id);
        }

        [HttpPost]
        [Route("api/UserProfile/{id}/likes/places")]
        public UrerActionResult AddPlace([FromUri] Guid id, [FromBody] object data)
        {
            return userProfileRestClient.AddPlace(id, data);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/likes/places/{placeId}")]
        public UrerActionResult RemovePlace([FromUri] Guid id, [FromUri] Guid placeId)
        {
            return userProfileRestClient.RemovePlace(id, placeId);
        }

        #endregion

        #region Preferences

        [HttpGet]
        [Route("api/UserProfile/{id}/preferences/movies")]
        public UrerActionResult GetMoviePrefrences([FromUri] Guid id)
        {
            return userProfileManager.GetMoviePreferences(id);
        }

        [HttpPost]
        [Route("api/UserProfile/{id}/preferences/movies")]
        public UrerActionResult AddMoviePreferences([FromUri] Guid id, [FromBody] Guid movieId)
        {
            return userProfileRestClient.AddMoviePreference(id, movieId);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/preferences/movies/{movieId}")]
        public UrerActionResult RemoveMoviePreferences([FromUri] Guid id, [FromUri] Guid movieId)
        {
            return userProfileRestClient.RemoveMoviePreferece(id, movieId);
        }

        [HttpGet]
        [Route("api/UserProfile/{id}/preferences/places")]
        public UrerActionResult GetPlacesPreferences([FromUri] Guid id)
        {
            return userProfileManager.GetPlacesPreferences(id);
        }

        [HttpPost]
        [Route("api/UserProfile/{id}/preferences/places")]
        public UrerActionResult AddPlacePreferences([FromUri] Guid id, [FromBody] Guid placeId)
        {
            return userProfileRestClient.AddPlacePreference(id, placeId);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/preferences/places/{placeId}")]
        public UrerActionResult RemovePlacePreferences([FromUri] Guid id, [FromUri] Guid placeId)
        {
            return userProfileRestClient.RemovePlacePreference(id, placeId);
        }

        #endregion
    }
}