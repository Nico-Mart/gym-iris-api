using Backend_Gym_Iris.DTOs.Admin;
using Backend_Gym_Iris.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend_Gym_Iris.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminsController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminsController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidatePassword([FromBody] ValidatePasswordRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new { message = "La contraseña es requerida" });
            }

            var isValid = await _adminService.ValidatePasswordAsync(request.Password);

            if (!isValid)
            {
                return Unauthorized(new { message = "Contraseña incorrecta" });
            }

            return Ok(new { message = "Acceso autorizado", isValid = true });
        }
    }
}
