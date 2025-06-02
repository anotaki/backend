using anotaki_api.Data;
using anotaki_api.DTOs.Requests.Address;
using anotaki_api.Models;
using anotaki_api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace anotaki_api.Services
{
    public class AddressService(AppDbContext context) : IAddressService
    {

        private readonly AppDbContext _context = context;

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
            }
            else if (address.IsStandard)
            {
                bool isOnlyStandard = user.Addresses.Count(a => a.IsStandard) == 1;
                if (isOnlyStandard)
                    throw new Exception("Cannot unset the only standard address. At least one must remain.");
                address.IsStandard = false;
            }

            await _context.SaveChangesAsync();

        }



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
