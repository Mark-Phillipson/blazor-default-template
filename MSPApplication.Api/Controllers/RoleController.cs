using Microsoft.AspNetCore.Mvc;
using MSPApplication.Data.Repositories;
using MSPApplication.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MSPApplication.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        // GET: api/<RoleController>
        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var result = _roleRepository.GetAllRoles();
            return Ok(result);
        }

        // GET api/<RoleController>/5
        [HttpGet("{id}")]
        public IActionResult GetRoleById(string id)
        {
            var result = _roleRepository.GetRoleById(id);
            return Ok(result);
        }

        // POST api/<RoleController>
        [HttpPost]
        public IActionResult CreateRole([FromBody] AspNetRole role)
        {
            if (role == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(role.Name))
            {
                ModelState.AddModelError("Name", "The Role Name should not be empty! ");
            }
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdRole = _roleRepository.AddRole(role);
            return Created("role", createdRole);
        }

        // PUT api/<RoleController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateRole([FromBody] AspNetRole role)
        {
            if (role == null)
            {
                return BadRequest();
            }
            if (string.IsNullOrEmpty(role.Name))
            {
                ModelState.AddModelError("Name", "The Rolename should not be empty! ");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var roleToUpdate = _roleRepository.GetRoleById(role.Id);
            if (roleToUpdate == null)
            {
                return NotFound();
            }
            _roleRepository.UpdateRole(role);
            return NoContent();//success
        }

        // DELETE api/<RoleController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteRole(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }
            var roleToDelete = _roleRepository.GetRoleById(id);
            if (roleToDelete == null) return NotFound();
            _roleRepository.DeleteRole(id);
            return NoContent();//success
        }
    }
}
