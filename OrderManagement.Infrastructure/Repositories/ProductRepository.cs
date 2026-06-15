using OrderManagement.Core.Entities;
using OrderManagement.Core.Interfaces;
using OrderManagement.Infrastructure.Data;

namespace OrderManagement.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(AppDbContext context) : base(context) { }
}
