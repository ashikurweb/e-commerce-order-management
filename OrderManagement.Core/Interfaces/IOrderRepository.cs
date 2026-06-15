using OrderManagement.Core.Entities;

namespace OrderManagement.Core.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order?> GetOrderWithDetailsAsync(int id);
    Task<IEnumerable<Order>> GetOrdersByCustomerAsync(int customerId);
}