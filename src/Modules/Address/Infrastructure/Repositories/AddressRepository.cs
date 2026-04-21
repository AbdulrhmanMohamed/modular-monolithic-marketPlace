namespace Modules.Address.Infrastructure.Repositories;

using Microsoft.EntityFrameworkCore;
using Modules.Address.Domain.Entities;
using Modules.Address.Domain.Interfaces;
using Modules.Address.Infrastructure.Data;

public class AddressRepository : IAddressRepository
{
    private readonly AddressDbContext _context;

    public AddressRepository(AddressDbContext context)
    {
        _context = context;
    }

    public async Task<List<Address>> GetByUserIdAsync(int userId)
    {
        return await _context.Addresses
            .Where(a => a.UserId == userId)
            .OrderBy(a => a.IsDefault)
            .ToListAsync();
    }

    public async Task<Address?> GetByIdAsync(int id)
    {
        return await _context.Addresses.FindAsync(id);
    }

    public async Task<Address> CreateAsync(Address address)
    {
        _context.Addresses.Add(address);
        await _context.SaveChangesAsync();
        return address;
    }

    public async Task<Address?> UpdateAsync(Address address)
    {
        _context.Addresses.Update(address);
        await _context.SaveChangesAsync();
        return address;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var address = await _context.Addresses.FindAsync(id);
        if (address is null) return false;
        _context.Addresses.Remove(address);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task SetDefaultAsync(int addressId, int userId)
    {
        var addresses = await _context.Addresses
            .Where(a => a.UserId == userId)
            .ToListAsync();

        foreach (var a in addresses)
        {
            a.IsDefault = a.Id == addressId;
        }

        await _context.SaveChangesAsync();
    }
}