namespace Modules.Address.Domain.Interfaces;

using Modules.Address.Domain.Entities;

public interface IAddressRepository
{
    Task<List<Address>> GetByUserIdAsync(int userId);
    Task<Address?> GetByIdAsync(int id);
    Task<Address> CreateAsync(Address address);
    Task<Address?> UpdateAsync(Address address);
    Task<bool> DeleteAsync(int id);
    Task SetDefaultAsync(int addressId, int userId);
}