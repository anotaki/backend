using anotaki_api.Data;
using anotaki_api.DTOs.Requests.User;
using anotaki_api.Exceptions;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using anotaki_api.Utils;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Services
{
    public class UserService(AppDbContext context) : IUserService
    {
        private readonly AppDbContext _context = context;

        public async Task<User?> FindById(int id)
        {
            return await _context.Users.Include(x => x.Addresses).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User?> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task<User?> FindByCpf(string cpf)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Cpf == cpf);
        }

        public async Task<User> CreateUser(CreateUserRequestDTO userDTO)
        {
            var newUser = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Cpf = userDTO.Cpf,
                Password = HashUtils.HashPassword(userDTO.Password),
                Role = Role.Default
            };

            if (await FindByCpf(newUser.Cpf) != null)
            {
                throw new CpfDuplicatedException();
            }

            if (await FindByEmail(newUser.Email) != null)
            {
                throw new EmailDuplicatedException();
            }

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return newUser;
        }

        public async Task<User> UpdateUser(UpdateUserRequestDTO dto, User user)
        {
            if (!string.IsNullOrWhiteSpace(dto.Name))
                user.Name = dto.Name;

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                // TODO: utils de regex pra senha forte
                user.Password = HashUtils.HashPassword(dto.Password);
            }

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeactivateUser(User user)
        {
            try
            {
                user.IsActive = false;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot deactivate user: {ex.Message}");
            }
        }
    }
}