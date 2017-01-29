using System;
using System.Configuration;
using UrerGateway.Business.Models;

namespace UrerGateway.Business.RestClients
{
    public class UserProfileRestClient : BaseRestClient
    {
        private const string Path = "UserProfile";

        public UserProfileRestClient()
        {
            var address = ConfigurationManager.AppSettings["UserProfileLink"];
            this.BaseAddress = new Uri(address);
        }

        #region User

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

        #endregion

        #region Friends

        public UrerActionResult GetFriends(Guid id)
        {
            var path = Path + "/" + id + "/friends";
            return base.Get(path);
        }

        public UrerActionResult AddFriend(Guid id, Guid friendId)
        {
            var path = Path + "/" + id + "/friends/" + friendId;
            return this.Put(path);
        }

        public UrerActionResult RemoveFriend(Guid id, Guid friendId)
        {
            var path = Path + "/" + id + "/friends/" + friendId;
            return this.Delete(path);
        }

        #endregion

        #region Enemies

        public UrerActionResult GetEnemies(Guid id)
        {
            var path = Path + "/" + id + "/enemies";
            return base.Get(path);
        }

        public UrerActionResult AddEnemy(Guid id, Guid enemyId)
        {
            var path = Path + "/" + id + "/enemies/" + enemyId;
            return this.Put(path);
        }

        public UrerActionResult RemoveEnemy(Guid id, Guid enemyId)
        {
            var path = Path + "/" + id + "/enemies/" + enemyId;
            return this.Delete(path);
        }

        #endregion

        #region Likes

        public UrerActionResult GetLikedMovies(Guid id)
        {
            var path = Path + "/" + id + "/likes/movies";
            return base.Get(path);
        }

        public UrerActionResult AddMovie(Guid id, Guid movieId)
        {
            var path = Path + "/" + id + "/likes/movies/" + movieId;
            return this.Put(path);
        }

        public UrerActionResult RemoveMovie(Guid id, Guid movieId)
        {
            var path = Path + "/" + id + "/likes/movies/" + movieId;
            return this.Delete(path);
        }

        public UrerActionResult GetLikedPlaces(Guid id)
        {
            var path = Path + "/" + id + "/likes/places";
            return base.Get(path);
        }

        public UrerActionResult AddPlace(Guid id, Guid placeId)
        {
            var path = Path + "/" + id + "/likes/places/" + placeId;
            return this.Put(path);
        }

        public UrerActionResult RemovePlace(Guid id, Guid placeId)
        {
            var path = Path + "/" + id + "/likes/places/" + placeId;
            return this.Delete(path);
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
            var path = Path + "/" + id + "/preferences/places/" + placeId;
            return this.Put(path);
        }

        public UrerActionResult RemovePlacePreference(Guid id, Guid placeId)
        {
            var path = Path + "/" + id + "/preferences/places/" + placeId;
            return this.Delete(path);
        }

        public UrerActionResult AddMoviePreference(Guid id, Guid movieId)
        {
            var path = Path + "/" + id + "/preferences/movies/" + movieId;
            return this.Put(path);
        }

        public UrerActionResult RemoveMoviePreferece(Guid id, Guid movieId)
        {
            var path = Path + "/" + id + "/preferences/movies/" + movieId;
            return this.Delete(path);
        }

        #endregion
    }
}