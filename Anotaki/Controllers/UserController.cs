using anotaki_api.DTOs.Requests.Address;
using anotaki_api.DTOs.Requests.User;
using anotaki_api.DTOs.Response.User;
using anotaki_api.Models.Response;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static anotaki_api.Utils.ClaimUtils;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace anotaki_api.Controllers
{
    [ApiController]
    [Route("/api/v1/users")]
    [Authorize]
    public class UserController(IUserService userService) : ControllerBase
    {

        /**************************************************
         * 
         *                  User
         * 
         **************************************************/
        private readonly IUserService _userService = userService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO userDTO)
        {
            var user = await _userService.CreateUser(userDTO);

            var data = new UserResponseDTO
            {
                Name = user.Name,
                Cpf = user.Cpf,
                Email = user.Email
            };

            return ApiResponse.Create("User created successfully!", StatusCodes.Status201Created, data);

        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentUserInfo()
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

            // mapeando endereço
            var defaultAddress = user.Addresses
                .Where(a => a.IsStandard)
                .Select(a => new UserAddressResponseDTO
                {
                    Id = a.Id,
                    Street = a.Street,
                    Number = a.Number,
                    City = a.City,
                    State = a.State,
                    ZipCode = a.ZipCode,
                    Neighborhood = a.Neighborhood,
                    Complement = a.Complement,
                    IsStandard = a.IsStandard
                })
                .FirstOrDefault();

            var data = new UserResponseDTO
            {
                Cpf = user.Cpf,
                Email = user.Email,
                Name = user.Name,
                DefaultAddress = defaultAddress
            };

            return ApiResponse.Create("Reading User Information", StatusCodes.Status200OK, data);
        }

        /**************************************************
         * 
         *                  Address
         * 
         **************************************************/

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

                var data = new UserAddressResponseDTO
                {
                    Street = createdAddress.Street,
                    Number = createdAddress.Number,
                    City = createdAddress.City,
                    ZipCode = createdAddress.ZipCode,
                    Neighborhood = createdAddress.Neighborhood,
                    Complement = createdAddress.Complement ?? string.Empty,
                    IsStandard = createdAddress.IsStandard
                };

                return ApiResponse.Create("Address saved!", StatusCodes.Status200OK, data);
            }
            catch (DbUpdateException ex)
            {
                return ApiResponse.Create("Failed to save address!", StatusCodes.Status400BadRequest, ex.Message);
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
                return ApiResponse.Create("User not authenticated.", StatusCodes.Status401Unauthorized);
            }

            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);
            }

            try
            {
                await _userService.UpdateUserAddress(user, addressId, addressDTO);
                return ApiResponse.Create("Address updated successfully.", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to update address.", StatusCodes.Status400BadRequest, ex.Message);
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
                return ApiResponse.Create("User not authenticated.", StatusCodes.Status401Unauthorized);
            }


            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);
            }

            try
            {
                await _userService.GetAllUserAddress(user);
            }
            catch (DbUpdateException ex)
            {
                return ApiResponse.Create("Failed to retrieve addresses.", StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Unexpected error occurred.", StatusCodes.Status400BadRequest, ex.Message);
            }

            var data = user.Addresses.Select(a => new UserAddressResponseDTO
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

            return ApiResponse.Create("User addresses fetched successfully.", StatusCodes.Status200OK, data);

        }


        [HttpDelete]
        [Route("address")]
        [Authorize]
        public async Task<IActionResult> DeleteUserAddress([FromBody] int addressId)
        {
            var userId = ClaimsUtils.GetUserId(User);
            if (userId == null)
            {
                return ApiResponse.Create("User not authenticated.", StatusCodes.Status401Unauthorized);
            }
            var user = await _userService.FindById(userId.Value);
            if (user == null)
            {
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);
            }

            try
            {
                await _userService.DeleteUserAddress(user, addressId);
                return ApiResponse.Create("Address removed successfully.", StatusCodes.Status200OK);
            }
            catch (DbUpdateException ex)
            {
                return ApiResponse.Create("Failed to remove address from database.", StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Unexpected error occurred.", StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}
