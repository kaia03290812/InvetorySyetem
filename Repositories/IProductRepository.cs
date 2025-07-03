using InventorySyetem.Models;

namespace InventorySyetem1.Repositories;

public interface IProductRepository
{
 List<Product> GetAllProducts();
 Product? GetProductById(string? id);
}