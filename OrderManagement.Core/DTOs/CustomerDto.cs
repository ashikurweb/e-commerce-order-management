namespace OrderManagement.Core.DTOs;

public record CustomerDto(int Id, string Name, string Email, string Phone);

public record CreateCustomerDto(string Name, string Email, string Phone);

public record UpdateCustomerDto(string Name, string Email, string Phone);