using Microsoft.EntityFrameworkCore;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Infrastructure.Data;

namespace OrderManagement.Infrastructure.Repositories;

public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
{
    public CustomerRepository(AppDbContext context) : base(context) { }

    public async Task<Customer?> GetByEmailAsync(string email) =>
        await _context.Customers.FirstOrDefaultAsync(c => c.Email == email);

    public async Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync() =>
        await _context.Customers.Include(c => c.Orders).ToListAsync();
}
