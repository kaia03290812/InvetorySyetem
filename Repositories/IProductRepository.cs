using InventorySyetem.Models;

namespace InventorySyetem1.Repositories;

public interface IProductRepository
{
 List<Product> GetAllProducts();

 Product GetProductById(int id);

 // void AddProduct(string? name, decimal price, int quantity);
 int GetNextProductId();
 void AddProduct(Product product);
 void UpdateProduct(Product product);
}