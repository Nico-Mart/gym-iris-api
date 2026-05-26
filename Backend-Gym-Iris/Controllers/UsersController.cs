using Backend_Gym_Iris.DTOs.User;
using Backend_Gym_Iris.Entities;
using Backend_Gym_Iris.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Gym_Iris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            var users = await _userService.GetAllUsersWithPaymentsAsync();
            return Ok(users.Select(_userService.MapToResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponse>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });

            return Ok(_userService.MapToResponse(user));
        }

        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserResponse>> GetUserByEmail(string email)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
                return NotFound(new { message = $"Usuario con email {email} no encontrado" });

            return Ok(_userService.MapToResponse(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Telephone = request.Telephone,
                Activity = request.Activity
            };
             
            try
            {
                try
                {
                    var created = await _userService.CreateUserAsync(user);
                    var full = await _userService.GetUserByIdAsync(created.Id);
                    return CreatedAtAction(nameof(GetUser), new { id = created.Id }, _userService.MapToResponse(full!));
                }
                catch (InvalidOperationException ex)
                {
                    return Conflict(new { message = ex.Message });
                }
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = new User
            {
                Id = id,
                Name = request.Name,
                Email = request.Email,
                Telephone = request.Telephone,
                Activity = request.Activity,
                Status = request.Status
            };

            var updated = await _userService.UpdateUserAsync(id, user);
            if (updated == null) return NotFound();

            var full = await _userService.GetUserByIdAsync(id);
            return Ok(_userService.MapToResponse(full!)); 
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUserAsync(id);
            if (!result)
                return NotFound(new { message = $"Usuario con ID {id} no encontrado" });

            return NoContent();
        }
    }
}