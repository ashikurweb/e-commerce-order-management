using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Core.Services;

public interface ICustomerService
{
    Task<IEnumerable<CustomerDto>> GetAllAsync();
    Task<CustomerDto?> GetByIdAsync(int id);
    Task<CustomerDto> CreateAsync(CreateCustomerDto dto);
    Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(int id);
}

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repo;

    public CustomerService(ICustomerRepository repo) => _repo = repo;

    public async Task<IEnumerable<CustomerDto>> GetAllAsync()
    {
        var customers = await _repo.GetAllAsync();
        return customers.Select(MapToDto);
    }

    public async Task<CustomerDto?> GetByIdAsync(int id)
    {
        var customer = await _repo.GetByIdAsync(id);
        return customer is null ? null : MapToDto(customer);
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerDto dto)
    {
        var customer = new Customer
        {
            Name = dto.Name,
            Email = dto.Email,
            Phone = dto.Phone
        };
        var created = await _repo.CreateAsync(customer);
        return MapToDto(created);
    }

    public async Task<CustomerDto?> UpdateAsync(int id, UpdateCustomerDto dto)
    {
        var customer = await _repo.GetByIdAsync(id);
        if (customer is null) return null;

        customer.Name = dto.Name;
        customer.Email = dto.Email;
        customer.Phone = dto.Phone;
        await _repo.UpdateAsync(customer);
        return MapToDto(customer);
    }

    public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static CustomerDto MapToDto(Customer c) =>
        new(c.Id, c.Name, c.Email, c.Phone);
}
