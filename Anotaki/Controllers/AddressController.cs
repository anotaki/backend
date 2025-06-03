﻿using anotaki_api.DTOs.Requests.Address;
using anotaki_api.DTOs.Response.Api;
using anotaki_api.DTOs.Response.User;
using anotaki_api.Services.Interfaces;
using anotaki_api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Controllers
{
    [Route("api/v1/address")]
    [ApiController]
    [Authorize]
    public class AddressController(IUserService userService, IAddressService addressService) : ControllerBase
    {

        private readonly IUserService _userService = userService;
        private readonly IAddressService _addressService = addressService;


        [HttpPost]
        public async Task<IActionResult> CreateAddress(CreateAddressRequestDTO addressDTO)
        {
            var userId = ClaimUtils.GetUserId(User);
            if (userId == null)
                return Unauthorized(new { message = "User not authenticated." });


            var user = await _userService.FindById(userId.Value);
            if (user == null)
                return NotFound(new { message = "User not found." });

            try
            {
                var createdAddress = await _addressService.CreateAddress(user, addressDTO);

                var data = new AddressResponseDTO
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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateAddress([FromRoute] int id, [FromBody] UpdateAddressRequestDTO addressDTO)
        {
            var userId = ClaimUtils.GetUserId(User);
            if (userId == null)
                return ApiResponse.Create("User not authenticated.", StatusCodes.Status401Unauthorized);


            var user = await _userService.FindById(userId.Value);
            if (user == null)

                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);


            var address = user.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                return ApiResponse.Create("Address not belong to user.", StatusCodes.Status404NotFound);

            try
            {
                var updatedAddress = await _addressService.UpdateUserAddress(id, addressDTO, user);
                var data = new AddressResponseDTO
                {
                    Street = updatedAddress.Street,
                    Number = updatedAddress.Number,
                    City = updatedAddress.City,
                    ZipCode = updatedAddress.ZipCode,
                    Neighborhood = updatedAddress.Neighborhood,
                    Complement = updatedAddress.Complement ?? string.Empty,
                    IsStandard = updatedAddress.IsStandard
                };

                return ApiResponse.Create("Address updated successfully.", StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Failed to update address.", StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpPatch("set-standard/{id}")]
        public async Task<IActionResult> SetStandardAddress([FromQuery] bool flag, [FromRoute] int id)
        {
            var userId = ClaimUtils.GetUserId(User);
            if (userId == null)
                return ApiResponse.Create("User not authenticated.", StatusCodes.Status401Unauthorized);

            var user = await _userService.FindById(userId.Value);
            if (user == null)
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

            var address = user.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                return ApiResponse.Create("Address not belong to user.", StatusCodes.Status404NotFound);

            try
            {
                var updatedAddress = await _addressService.SetStandardAddress(flag, id, userId.Value);
                var data = new AddressResponseDTO
                {
                    Street = updatedAddress.Street,
                    Number = updatedAddress.Number,
                    City = updatedAddress.City,
                    ZipCode = updatedAddress.ZipCode,
                    Neighborhood = updatedAddress.Neighborhood,
                    Complement = updatedAddress.Complement ?? string.Empty,
                    IsStandard = updatedAddress.IsStandard
                };

                return ApiResponse.Create("Standard address updated successfully.", StatusCodes.Status200OK, data);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Unexpected error occurred.", StatusCodes.Status400BadRequest, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAddress([FromRoute] int id)
        {
            var userId = ClaimUtils.GetUserId(User);
            if (userId == null)
                return ApiResponse.Create("User not authenticated.", StatusCodes.Status401Unauthorized);

            var user = await _userService.FindById(userId.Value);
            if (user == null)
                return ApiResponse.Create("User not found.", StatusCodes.Status404NotFound);

            var address = user.Addresses.FirstOrDefault(a => a.Id == id);
            if (address == null)
                return ApiResponse.Create("Address not belong to user.", StatusCodes.Status404NotFound);

            try
            {
                await _addressService.DeleteUserAddress(user, id);
                return ApiResponse.Create("Address removed successfully.", StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return ApiResponse.Create("Unexpected error occurred.", StatusCodes.Status400BadRequest, ex.Message);
            }
        }
    }
}
