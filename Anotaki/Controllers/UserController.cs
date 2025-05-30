﻿using anotaki_api.DTOs.Requests;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Controllers
{
    [ApiController]
    [Authorize(Roles = Roles.Admin)]
    [Route("/api/v1/users")]
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
    }
}