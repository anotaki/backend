using anotaki_api.DTOs.Requests;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;

namespace anotaki_api.Controllers
{
    [ApiController]
    [Route("/api/v1/users")]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO userDTO)
        {
            var user = await _userService.CreateUser(userDTO);

            return StatusCode(StatusCodes.Status201Created, new
            {
                message = "User created successfully.",
                name = user.Name,
                cpf = user.Cpf,
                email = user.Email
            });
        }

        [HttpPost]
        [Route("address")]
        public async Task<IActionResult> CreateAddress(CreateAddressDTO addressDTO)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var user = await _userService.FindById(userId);

            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            try
            {
                await _userService.CreateAddress(user, addressDTO);
            } catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Failed to save address.", error = ex.Message });
            }

            return Ok($"Address saved in user #{userId}");
        }
    }
}