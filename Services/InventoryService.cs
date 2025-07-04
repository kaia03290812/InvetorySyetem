using InventorySyetem.Models;
using InventorySyetem.Utils;
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
   var emailNotifier = new EmailNotifier();
   var emailServise = new NotificationService(emailNotifier);
   //呼叫介面,而非實作
   List<Product> products = _productRepository.GetAllProducts();
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