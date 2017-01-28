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
            Manager = new ManagerUpdateDb();
        }

        public ManagerUpdateDb Manager { get; set; }

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

        [HttpGet]
        [Route("api/UserProfile/{id}/friends")]
        public IHttpActionResult GetFriends([FromUri] Guid id)
        {
            var result = Manager.GetFriends(id);
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

            var friendsExists = Manager.CheckUserExistsById(friendId);
            if (!friendsExists)
            {
                return BadRequest("User does not exists");
            }

            Manager.AddFriend(id, friendId);
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

            var friendsExists = Manager.CheckUserExistsById(friendId);
            if (!friendsExists)
            {
                return BadRequest("User does not exists");
            }

            Manager.RemoveFriend(id, friendId);
            return Ok("friend removed");
        }

    }
}
