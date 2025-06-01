using anotaki_api.Data;
using anotaki_api.DTOs.Requests;
using anotaki_api.Exceptions;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using anotaki_api.Utils;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace anotaki_api.Services
{
    public class UserService(AppDbContext context) : IUserService
    {
        private readonly AppDbContext _context = context;

        public async Task<User> FindById(int id)
        {
            return await _context.Users.Include(x=> x.Addresses).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

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

        public async Task CreateAddress(User user, CreateAddressDTO addressDTO)
        {

            bool isFirstAddress = user.Addresses.Count == 0;

            var address = new Address
            {
                UserId = user.Id,
                Street = addressDTO.Street,
                Number = addressDTO.Number,
                City = addressDTO.City,
                State = addressDTO.State,
                ZipCode = addressDTO.ZipCode,
                Neighborhood = addressDTO.Neighborhood,
                Complement = addressDTO.Complement,
                IsStandard = isFirstAddress
            };

            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();
        }
    }
}