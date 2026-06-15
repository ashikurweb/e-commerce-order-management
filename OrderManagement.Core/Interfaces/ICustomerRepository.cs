using OrderManagement.Core.Entities;

namespace OrderManagement.Core.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync();
}