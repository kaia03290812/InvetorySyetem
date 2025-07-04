using InventorySyetem.Models;
using InventorySyetem1.Repositories;

namespace InventorySyetem.Services;

public class InventoryService
{
 private static IProductRepository _productRepository;

 public InventoryService(IProductRepository productRepository)
 {
  _productRepository = productRepository;
 }

 public static List<Product> GetAllProducts()
 {
  try
  {
   //呼叫介面,而非實作
   var products = _productRepository.GetAllProducts();
   if (!products.Any()) Console.WriteLine("No products found");

   return products;
  }
  catch (Exception e)
  {
   Console.WriteLine($"讀取產品列表失敗:{e.Message}");
   return new List<Product>();
  }
 }
}