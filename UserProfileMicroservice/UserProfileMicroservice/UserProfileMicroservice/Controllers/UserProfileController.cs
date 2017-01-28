using System;
using System.Web.Http;
using Framework.Common;
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
        }

        public EnemiesManager EnemiesManager { get; set; }

        public FriendsManager FriendsManager { get; set; }

        public UserManager Manager { get; set; }

        #region User

        [HttpPost]
        public IHttpActionResult Register(UserModel user)
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


        #endregion    #region Friends

        #region Enemies

        [HttpGet]
        [Route("api/UserProfile/{id}/enemies")]
        public IHttpActionResult GetEnemy([FromUri] Guid id)
        {
            var result = EnemiesManager.GetEnemies(id);
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
    }
}
