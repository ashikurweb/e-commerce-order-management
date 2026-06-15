using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Core.Services;

public interface IOrderService
{
    Task<IEnumerable<OrderDto>> GetAllAsync();
    Task<OrderDto?> GetByIdAsync(int id);
    Task<OrderDto> CreateAsync(CreateOrderDto dto);
    Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
    Task<bool> DeleteAsync(int id);
}

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepo;
    private readonly IProductRepository _productRepo;

    public OrderService(IOrderRepository orderRepo, IProductRepository productRepo)
    {
        _orderRepo = orderRepo;
        _productRepo = productRepo;
    }

    public async Task<IEnumerable<OrderDto>> GetAllAsync()
    {
        var orders = await _orderRepo.GetAllAsync();
        return orders.Select(MapToDto);
    }

    public async Task<OrderDto?> GetByIdAsync(int id)
    {
        var order = await _orderRepo.GetOrderWithDetailsAsync(id);
        return order is null ? null : MapToDto(order);
    }

    public async Task<OrderDto> CreateAsync(CreateOrderDto dto)
    {
        var items = new List<OrderItem>();
        decimal total = 0;

        foreach (var item in dto.Items)
        {
            var product = await _productRepo.GetByIdAsync(item.ProductId)
                ?? throw new KeyNotFoundException($"Product {item.ProductId} not found");

            items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                UnitPrice = product.Price
            });
            total += product.Price * item.Quantity;
        }

        var order = new Order
        {
            CustomerId = dto.CustomerId,
            TotalAmount = total,
            OrderItems = items
        };

        var created = await _orderRepo.CreateAsync(order);
        var full = await _orderRepo.GetOrderWithDetailsAsync(created.Id);
        return MapToDto(full!);
    }

    public async Task<OrderDto?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
    {
        var order = await _orderRepo.GetByIdAsync(id);
        if (order is null) return null;

        order.Status = dto.Status;
        await _orderRepo.UpdateAsync(order);
        return MapToDto(order);
    }

    public async Task<bool> DeleteAsync(int id) => await _orderRepo.DeleteAsync(id);

    private static OrderDto MapToDto(Order o) => new(
        o.Id,
        o.CustomerId,
        o.Customer?.Name ?? "",
        o.OrderDate,
        o.Status,
        o.TotalAmount,
        o.OrderItems.Select(oi => new OrderItemDto(
            oi.ProductId,
            oi.Product?.Name ?? "",
            oi.Quantity,
            oi.UnitPrice
        )).ToList()
    );
}