using anotaki_api.Data;
using anotaki_api.DTOs.Requests;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using anotaki_api.Utils;
using Microsoft.EntityFrameworkCore;
using System;


namespace anotaki_api.Services
{
    public class UserService(AppDbContext context) : IUserService
    {
        private readonly AppDbContext _context = context;

        public async Task<User> FindByCpf(string cpf)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Cpf == cpf);
        }

        public async Task<User> CreateUser(CreateUserDTO userDTO)
        {
            var newUser = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Cpf = userDTO.Cpf,
                Password = HashUtils.HashPassword(userDTO.Password),

            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }
    }
}