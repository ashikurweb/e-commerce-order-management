namespace OrderManagement.Core.DTOs;

public record OrderItemDto(int ProductId, string ProductName, int Quantity, decimal UnitPrice);

public record OrderDto(
    int Id,
    int CustomerId,
    string CustomerName,
    DateTime OrderDate,
    string Status,
    decimal TotalAmount,
    List<OrderItemDto> Items
);

public record CreateOrderItemDto(int ProductId, int Quantity);

public record CreateOrderDto(int CustomerId, List<CreateOrderItemDto> Items);

public record UpdateOrderStatusDto(string Status);