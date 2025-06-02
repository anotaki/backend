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

        // PUBLIC ASYNC 
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

        public async Task<Address> CreateAddress(User user, CreateAddressRequestDTO addressDTO)
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

            return address;
        }

        public async Task<List<Address>> GetAllUserAddress(User user)
        {
            return await _context.Addresses.Where(x => x.UserId == user.Id).ToListAsync();
        }

        public async Task DeleteUserAddress(User user, int addresId)
        {
            Address? address = user.Addresses.Find(a => a.Id == addresId);
            if (address is null)
                throw new Exception("Address not found or does not belong to user.");

            try
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Cannot remove User Address: {ex}");
            }
        }

        public async Task UpdateUserAddress(User user, int addressId, UpdateAddressRequestDTO dto)
        {
            var address = user.Addresses.FirstOrDefault(a => a.Id == addressId);
            if (address is null)
                throw new Exception("Address not found or does not belong to user.");

            if (dto.City != null) address.City = dto.City;
            if (dto.State != null) address.State = dto.State;
            if (dto.ZipCode != null) address.ZipCode = dto.ZipCode;
            if (dto.Neighborhood != null) address.Neighborhood = dto.Neighborhood;
            if (dto.Street != null) address.Street = dto.Street;
            if (dto.Number != null) address.Number = dto.Number;
            if (dto.Complement != null) address.Complement = dto.Complement;

            // alterar endereço padrão
            if (dto.IsStandard.HasValue && dto.IsStandard.Value)
            {
                SetStandardAddress(user.Addresses, address.Id);
            } else if (address.IsStandard)
            {
                bool isOnlyStandard = user.Addresses.Count(a => a.IsStandard) == 1;
                if (isOnlyStandard)
                    throw new Exception("Cannot unset the only standard address. At least one must remain.");
                address.IsStandard = false;
            }

            await _context.SaveChangesAsync();

        }



        // PRIVATE FUNCTIONS
        private static void SetStandardAddress(List<Address> addresses, int standardAddressId)
        {
            bool addressFound = false;

            foreach (var addr in addresses)
            {
                if (addr.Id == standardAddressId)
                {
                    addr.IsStandard = true;
                    addressFound = true;
                }
                else
                {
                    addr.IsStandard = false;
                }
            }

            if (!addressFound)
            {
                throw new Exception("Address to set as standard not found.");
            }
        }

    }
}