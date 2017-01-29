using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Framework.Common;
using Microsoft.Ajax.Utilities;
using UsersDBUpdate;

namespace UserProfileMicroservice.Controllers
{
    public class UserProfileController : ApiController
    {
        public UserProfileController()
        {
            Manager = new UserManager();
            FriendsManager = new FriendsManager();
            EnemiesManager = new EnemiesManager();
            LikesManager = new LikesManager();
            PreferencesManager = new PreferencesManager();
            LoggedInUsers = new List<LoggedInUser>();
        }

        public List<LoggedInUser> LoggedInUsers { get; set; }

        public EnemiesManager EnemiesManager { get; set; }

        public FriendsManager FriendsManager { get; set; }

        public UserManager Manager { get; set; }

        public LikesManager LikesManager { get; set; }

        public PreferencesManager PreferencesManager { get; set; }

        #region User

        [HttpPost]
        [Route("api/UserProfile/Register")]
        public IHttpActionResult Register(UserLoginModel user)
        {

            if (user == null)
            {
                return BadRequest();
            }

            var result = Manager.AddUser(user);

            if (result != null)
            {
                return Ok(result);
            }
            return BadRequest("User already exists");
        }

        [HttpPost]
        [Route("api/UserProfile/Login")]
        public IHttpActionResult Login(UserLoginModel user)
        {

            if (user == null)
            {
                return BadRequest();
            }

            var result = Manager.CheckCredentials(user);

            if (result.IsNullOrWhiteSpace())
            {
                return BadRequest("wrong inputs. try again.");
            }

            var newUserLoggedIn = new LoggedInUser()
            {
                Id = new Guid(result),
                Latitude = user.Latitude,
                Longitude = user.Longitude,
                Name = user.Name,
                Token = Helper.GenerateToken(user.Name)
            };


            if (LoggedInUsers.FirstOrDefault(x => x.Token == newUserLoggedIn.Token) == null)
            {
                LoggedInUsers.Add(newUserLoggedIn);
            }

            return Ok(newUserLoggedIn.Token);
        }

        [HttpPost]
        [Route("api/UserProfile/Logout")]
        public IHttpActionResult Logout(string token)
        {
            var user = LoggedInUsers.FirstOrDefault(x => x.Token == token);

            if (user != null)
            {
                LoggedInUsers.Remove(user);
            }


            return Ok();
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var result = Manager.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("api/UserProfile/{id}")]
        public IHttpActionResult GetByid([FromUri] Guid id)
        {
            var result = Manager.Get(id);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        #endregion

        #region Friends

        [HttpGet]
        [Route("api/UserProfile/{id}/friends")]
        public IHttpActionResult GetFriends([FromUri] Guid id)
        {
            var result = FriendsManager.GetFriends(id);

            foreach (var friend in result)
            {
                var friendLoggedIn = LoggedInUsers.FirstOrDefault(x => x.Id == friend.Id);
                if (friendLoggedIn != null)
                {
                    friend.Longitude = friendLoggedIn.Longitude;
                    friend.Latitude = friendLoggedIn.Latitude;
                }
            }


            return Ok(result);
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/friends/{friendId}")]
        public IHttpActionResult AddFriend([FromUri] Guid id, [FromUri] Guid friendId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            var friendExists = Manager.CheckUserExistsById(friendId);
            if (!friendExists)
            {
                return BadRequest("Friend does not exists");
            }

            FriendsManager.AddFriend(id, friendId);
            return Ok("friend added");
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/friends/{friendId}")]
        public IHttpActionResult RemoveFriend([FromUri] Guid id, [FromUri] Guid friendId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            var friendExists = Manager.CheckUserExistsById(friendId);
            if (!friendExists)
            {
                return BadRequest("Friend does not exists");
            }

            FriendsManager.RemoveFriend(id, friendId);
            return Ok("friend removed");
        }

        #endregion  

        #region Enemies

        [HttpGet]
        [Route("api/UserProfile/{id}/enemies")]
        public IHttpActionResult GetEnemies([FromUri] Guid id)
        {
            var result = EnemiesManager.GetEnemies(id);

            foreach (var enemy in result)
            {
                var enemyLoggedIn = LoggedInUsers.FirstOrDefault(x => x.Id == enemy.Id);
                if (enemyLoggedIn != null)
                {
                    enemy.Longitude = enemyLoggedIn.Longitude;
                    enemy.Latitude = enemyLoggedIn.Latitude;
                }
            }


            return Ok(result);
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/enemies/{enemyId}")]
        public IHttpActionResult AddEnemy([FromUri] Guid id, [FromUri] Guid enemyId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            var enemyExists = Manager.CheckUserExistsById(enemyId);
            if (!enemyExists)
            {
                return BadRequest("Enemy does not exists");
            }

            EnemiesManager.AddEnemy(id, enemyId);
            return Ok("enemy marked");
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/enemies/{enemyId}")]
        public IHttpActionResult RemoveEnemy([FromUri] Guid id, [FromUri] Guid enemyId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            var enemyExists = Manager.CheckUserExistsById(enemyId);
            if (!enemyExists)
            {
                return BadRequest("Enemy does not exists");
            }

            EnemiesManager.RemoveEnemy(id, enemyId);
            return Ok("enemy removed");
        }

        #endregion

        #region Likes

        [HttpGet]
        [Route("api/UserProfile/{id}/likes/movies")]
        public IHttpActionResult GetMovieLikes([FromUri] Guid id)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            var result = LikesManager.GetMoviesLikes(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("api/UserProfile/{id}/likes/places")]
        public IHttpActionResult GetPlacesLikes([FromUri] Guid id)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            var result = LikesManager.GetPlacesLikes(id);

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/likes/movies/{movieId}")]
        public IHttpActionResult RemoveMovieLike([FromUri] Guid id, [FromUri] Guid movieId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            LikesManager.DeleteMovieLike(id, movieId);

            return Ok("Like removed");
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/likes/places/{placeId}")]
        public IHttpActionResult RemovePlaceLike([FromUri] Guid id, [FromUri] Guid placeId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            LikesManager.DeletePlaceLike(id, placeId);

            return Ok("Like removed");
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/likes/movies/{movieId}")]
        public IHttpActionResult AddMovieLike([FromUri] Guid id, [FromUri] Guid movieId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            LikesManager.AddMovieLike(id, movieId);

            return Ok("Like added");
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/likes/places/{placeId}")]
        public IHttpActionResult AddPlaceLike([FromUri] Guid id, [FromUri] Guid placeId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            LikesManager.AddPlaceLike(id, placeId);

            return Ok("Like added");
        }

        #endregion

        #region Preferences

        [HttpGet]
        [Route("api/UserProfile/{id}/preferences/movies")]
        public IHttpActionResult GetMoviePrefrences([FromUri] Guid id)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            var result = PreferencesManager.GetMoviesPreferences(id);

            return Ok(result);
        }

        [HttpGet]
        [Route("api/UserProfile/{id}/preferences/places")]
        public IHttpActionResult GetPlacesPrefrences([FromUri] Guid id)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            var result = PreferencesManager.GetPlacesPreferences(id);

            return Ok(result);
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/preferences/movies/{movieTypeId}")]
        public IHttpActionResult RemoveMoviePrefrence([FromUri] Guid id, [FromUri] Guid movieTypeId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            PreferencesManager.DeleteMoviePreference(id, movieTypeId);

            return Ok("Preference removed");
        }

        [HttpDelete]
        [Route("api/UserProfile/{id}/preferences/places/{placeTypeId}")]
        public IHttpActionResult RemovePlacePrefrence([FromUri] Guid id, [FromUri] Guid placeTypeId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            PreferencesManager.DeletePlacePreference(id, placeTypeId);

            return Ok("Preference removed");
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/preferences/movies/{movieTypeId}")]
        public IHttpActionResult AddMoviePrefrence([FromUri] Guid id, [FromUri] Guid movieTypeId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            PreferencesManager.AddMoviePreference(id, movieTypeId);

            return Ok("Preference added");
        }

        [HttpPut]
        [Route("api/UserProfile/{id}/preferences/places/{placeTypeId}")]
        public IHttpActionResult AddPlacePrefrence([FromUri] Guid id, [FromUri] Guid placeTypeId)
        {
            var userExists = Manager.CheckUserExistsById(id);
            if (!userExists)
            {
                return BadRequest("User does not exists");
            }

            PreferencesManager.AddPlacePreference(id, placeTypeId);

            return Ok("Preference added");
        }

        #endregion
    }
}
