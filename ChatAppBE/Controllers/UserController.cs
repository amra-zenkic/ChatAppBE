using ChatAppBE.Models.Models;
using ChatAppBE.Services.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace ChatAppBE.Controllers
{
    [Route("users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public IActionResult CreateUser()
        {
            var username = _userService.GenerateUniqueUsername();
            var user = new User
            {
                Username = username,
                Status = "online"
            };
            user = _userService.AddUser(user);
            return Ok(user);
        }
        [HttpPost("updateStatus/{id}")]
        public IActionResult UpdateUserStatusToOffline(string id)
        {
            _userService.UpdateUserStatusToOffline(id);
            return Ok();
        }

        [HttpGet("all")]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("active")]
        public IActionResult GetAllActiveUsers()
        {
            var users = _userService.GetAllActiveUsers();
            return Ok(users);
        }
        [HttpGet("privateChat/{id}")]
        public IActionResult GetAllPrivateChatStarted(string id)
        {
            var users = _userService.GetAllPrivateChatStarted(id);
            return Ok(users);
        }
        [HttpDelete("deleteAll")]
        public IActionResult DeleteAllUsers()
        {
            _userService.DeleteAllUsers();
            return Ok();
        }
    }
}
