using anotaki_api.DTOs.Requests.User;
using anotaki_api.DTOs.Response.User;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Security.Claims;
using static anotaki_api.Utils.ClaimUtils;

namespace anotaki_api.Controllers
{
    [ApiController]
    [Route("/api/v1/users")]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO userDTO)
        {
            var user = await _userService.CreateUser(userDTO);

            var response = new UserResponseDTO
            {
                Message = "User created successfully.",
                Name = user.Name,
                Cpf = user.Cpf,
                Email = user.Email
            };

            return StatusCode(StatusCodes.Status201Created, response);
        }

        [HttpPost]
        [Route("address")]
        [Authorize]
        public async Task<IActionResult> CreateAddress(CreateAddressRequestDTO addressDTO)
        {
            var userId = ClaimsUtils.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            try
            {
                var createdAddress = await _userService.CreateAddress(user, addressDTO);

                var response = new CreateAddressResponseDTO
                {
                    Message = $"Address saved for user #{userId}",
                    Address = new AddressResponseDTO
                    {
                        Street = createdAddress.Street,
                        Number = createdAddress.Number,
                        City = createdAddress.City,
                        State = createdAddress.State,
                        ZipCode = createdAddress.ZipCode,
                        Neighborhood = createdAddress.Neighborhood,
                        Complement = createdAddress.Complement,
                        IsStandard = createdAddress.IsStandard
                    }
                };

                return Ok(response);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Failed to save address.", error = ex.Message });
            }
        }

        [HttpPatch]
        [Route("address")]
        [Authorize]
        public async Task<IActionResult> UpdateAddress([FromQuery]int addressId, [FromBody] UpdateAddressRequestDTO addressDTO)
        {
            var userId = ClaimsUtils.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }

            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            try
            {
                await _userService.UpdateUserAddress(user, addressId, addressDTO);
                return Ok(new { message = "Address updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        [Route("address")]
        [Authorize]
        public async Task<IActionResult> GetAllUserAddress()
        {
            var userId = ClaimsUtils.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }


            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            try
            {
                await _userService.GetAllUserAddress(user);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Failed to retrieve addresses.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

            var response = user.Addresses.Select(a => new AddressResponseDTO
            {
                Id = a.Id,
                City = a.City,
                State = a.State,
                ZipCode = a.ZipCode,
                Neighborhood = a.Neighborhood,
                Street = a.Street,
                Number = a.Number,
                Complement = a.Complement,
                IsStandard = a.IsStandard
            }).ToList();

            return Ok(response);

        }


        [HttpDelete]
        [Route("address")]
        [Authorize]
        public async Task<IActionResult> DeleteUserAddress([FromBody] int addressId)
        {
            var userId = ClaimsUtils.GetUserId(User);
            if (userId == null)
            {
                return Unauthorized(new { message = "User not authenticated." });
            }
            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return NotFound(new { message = "User not found." });
            }

            try
            {
                await _userService.DeleteUserAddress(user, addressId);
                return Ok("Address removed");
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new { message = "Failed to remove address from database.", error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
