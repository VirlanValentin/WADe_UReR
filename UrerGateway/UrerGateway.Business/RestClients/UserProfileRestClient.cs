using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Results;
using UrerGateway.Business.Models;

namespace UrerGateway.Business.RestClients
{
    public class UserProfileRestClient : BaseRestClient
    {
        private const string Path = "UserProfile";

        public UserProfileRestClient()
        {
            var address = ConfigurationManager.AppSettings["UserProfileLink"];
            BaseAddress = new Uri(address);
        }

        #region User

        public IHttpActionResult GetQrCode(object data)
        {
            var path = "UserProfile/GenerateQR";

            var responseFromMicroservice = this.PostAsJsonAsync(path, data).Result;
            if (responseFromMicroservice.IsSuccessStatusCode)
            {
                var dataResponse = responseFromMicroservice.Content.ReadAsByteArrayAsync().Result;

                var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(dataResponse) };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");

                ResponseMessageResult response = new ResponseMessageResult(result);

                return response;
            }

            return new UrerActionResult(responseFromMicroservice.StatusCode, responseFromMicroservice.Content.ReadAsStringAsync().Result);
        }

        public UrerActionResult Get()
        {
            return base.Get(Path);
        }

        public UrerActionResult Get(Guid id)
        {
            var path = Path + "/" + id;
            return base.Get(path);
        }

        public UrerActionResult Post(object data)
        {
            return base.Post(Path, data);
        }

        public UrerActionResult Register(object data)
        {
            var path = Path + "/register" ;
            return base.Post(path, data);
        }

        public UrerActionResult Login(object data)
        {
            var path = Path + "/login" ;
            return base.Post(path, data);
        }

        public UrerActionResult Logout(object data)
        {
            var path = Path + "/logout" ;
            return base.Post(path, data);
        }

        #endregion

        #region Friends

        public UrerActionResult GetFriends(Guid id)
        {
            var path = Path + "/" + id + "/friends";
            return base.Get(path);
        }

        public UrerActionResult AddFriend(Guid id, object data)
        {
            var path = Path + "/" + id + "/friends";
            return Post(path, data);
        }

        public UrerActionResult RemoveFriend(Guid id, Guid friendId)
        {
            var path = Path + "/" + id + "/friends/" + friendId;
            return Delete(path);
        }

        #endregion

        #region Enemies

        public UrerActionResult GetEnemies(Guid id)
        {
            var path = Path + "/" + id + "/enemies";
            return base.Get(path);
        }

        public UrerActionResult AddEnemy(Guid id, object data)
        {
            var path = Path + "/" + id + "/enemies";
            return Post(path, data);
        }

        public UrerActionResult RemoveEnemy(Guid id, Guid enemyId)
        {
            var path = Path + "/" + id + "/enemies/" + enemyId;
            return Delete(path);
        }

        #endregion

        #region Likes

        public UrerActionResult GetLikedMovies(Guid id)
        {
            var path = Path + "/" + id + "/likes/movies";
            return base.Get(path);
        }

        public UrerActionResult AddMovie(Guid id, object data)
        {
            var path = Path + "/" + id + "/likes/movies";
            return Post(path, data);
        }

        public UrerActionResult RemoveMovie(Guid id, Guid movieId)
        {
            var path = Path + "/" + id + "/likes/movies/" + movieId;
            return Delete(path);
        }

        public UrerActionResult GetLikedPlaces(Guid id)
        {
            var path = Path + "/" + id + "/likes/places";
            return base.Get(path);
        }

        public UrerActionResult AddPlace(Guid id, object data)
        {
            var path = Path + "/" + id + "/likes/places";
            return Post(path, data);
        }

        public UrerActionResult RemovePlace(Guid id, Guid placeId)
        {
            var path = Path + "/" + id + "/likes/places/" + placeId;
            return Delete(path);
        }

        #endregion

        #region Preferences

        public UrerActionResult GetMoviePreferences(Guid id)
        {
            var path = Path + "/" + id + "/preferences/movies";
            return base.Get(path);
        }

        public UrerActionResult GetPlacesPreferences(Guid id)
        {
            var path = Path + "/" + id + "/preferences/places";
            return base.Get(path);
        }

        public UrerActionResult AddPlacePreference(Guid id, Guid placeId)
        {
            var path = Path + "/" + id + "/preferences/places";
            return Post(path, placeId);
        }

        public UrerActionResult RemovePlacePreference(Guid id, Guid placeId)
        {
            var path = Path + "/" + id + "/preferences/places/" + placeId;
            return Delete(path);
        }

        public UrerActionResult AddMoviePreference(Guid id, Guid movieId)
        {
            var path = Path + "/" + id + "/preferences/movies";
            return Post(path, movieId);
        }

        public UrerActionResult RemoveMoviePreferece(Guid id, Guid movieId)
        {
            var path = Path + "/" + id + "/preferences/movies/" + movieId;
            return Delete(path);
        }

        #endregion

    }
}