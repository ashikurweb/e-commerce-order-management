using OrderManagement.Core.DTOs;
using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;

namespace OrderManagement.Core.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync();
    Task<ProductDto?> GetByIdAsync(int id);
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto);
    Task<bool> DeleteAsync(int id);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    public ProductService(IProductRepository repo) => _repo = repo;

    public async Task<IEnumerable<ProductDto>> GetAllAsync()
    {
        var products = await _repo.GetAllAsync();
        return products.Select(MapToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(int id)
    {
        var product = await _repo.GetByIdAsync(id);
        return product is null ? null : MapToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
            Stock = dto.Stock
        };
        var created = await _repo.CreateAsync(product);
        return MapToDto(created);
    }

    public async Task<ProductDto?> UpdateAsync(int id, UpdateProductDto dto)
    {
        var product = await _repo.GetByIdAsync(id);
        if (product is null) return null;

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        await _repo.UpdateAsync(product);
        return MapToDto(product);
    }

    public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);

    private static ProductDto MapToDto(Product p) =>
        new(p.Id, p.Name, p.Description, p.Price, p.Stock);
}
