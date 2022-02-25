using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;
using MSPApplication.Shared.ViewModels;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSPApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Administration")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        // GET: api/<UserController>
        [HttpGet]
        public ActionResult<IEnumerable<User>> GetAllUsers()
        {
            var result = _userRepository.GetAllUsers();
            return Ok(result);
        }

        [Route("[action]/{id}")]
        [HttpGet]
        public IActionResult GetAllUsersInRole(string id)
        {
            var result = _userRepository.GetAllUsersInRole(id);
            return Ok(result);
        }

        [Route("[action]/{userId}")]
        [HttpGet]
        public IActionResult GetAllRolesForUser(string userId)
        {
            var result = _userRepository.GetAllRolesForUser(userId);
            return Ok(result);
        }
        // GET api/<UserController>/5
        [Route("[action]/{id}")]
        [HttpGet]
        public IActionResult GetUserById(string id)
        {
            var result = _userRepository.GetUserById(id);
            return Ok(result);
        }

        // POST api/<UserController>
        [HttpPost]
        public IActionResult CreateUser([FromBody] AspNetUser user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError("UserName", "The Username should not be empty!");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdUser = _userRepository.AddUser(user);
            return Created("user", createdUser);
        }

        // POST api/<UserController>
        [Route("[action]")]
        [HttpPost]
        public IActionResult AddUserRole([FromBody] AspNetUserRole userRole)
        {
            if (userRole == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(userRole.UserId))
            {
                ModelState.AddModelError("UserId", "The User ID should not be empty! ");
            }
            if (string.IsNullOrEmpty(userRole.RoleId))
            {
                ModelState.AddModelError("RoleId", "The Role ID should not be empty! ");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var createdUserRole = _userRepository.AddUserRole(userRole);
            return Created("UserRole", createdUserRole);

        }


        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromBody] AspNetUser user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(user.UserName))
            {
                ModelState.AddModelError("UserName", "The Username should not be empty!");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userToUpdate = _userRepository.GetUserById(user.Id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            _userRepository.UpdateUser(user);
            return NoContent();//success
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var userToDelete = _userRepository.GetUserById(id);
            if (userToDelete == null) return NotFound();
            _userRepository.DeleteUser(id);
            return NoContent();//success
        }
        [HttpDelete]
        //[Route("api/user/{userId}/{roleId}")]
        public IActionResult DeleteUserRole(string userId, string roleId)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleId))
            {
                return BadRequest("past in arguments cannot be empty");
            }
            var userRoleToDelete = _userRepository.GetUserRoleByIds(userId, roleId);
            if (userRoleToDelete == null) return NotFound();
            _userRepository.DeleteUserRole(userId, roleId);
            return Ok();//success
        }
    }
}
