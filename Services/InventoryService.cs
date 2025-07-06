using InventorySyetem.Models;
using InventorySyetem1.Repositories;

namespace InventorySyetem.Services;

public class InventoryService
{
 private readonly IProductRepository _productRepository;

 public InventoryService(IProductRepository productRepository)
 {
  _productRepository = productRepository;
 }

 public List<Product> GetAllProducts(int input)
 {
  try
  {
   // var emailNotifier = new EmailNotifier();
   // var emailServise = new NotificationService(emailNotifier);
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

 public Product GetProductById(int id)
 {
  try
  {
   var product = _productRepository.GetProductById(id);
   if (product == null) Console.WriteLine("No product found");

   return product;
  }
  catch (Exception e)
  {
   Console.WriteLine($"讀取產品列表失敗:{e.Message}");
   return new Product();
  }
 }

 public void AddProduct(string? name, decimal price, int quantity)
 {
  try
  {
   if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("產品名稱不能為空");

   if (price <= 0) throw new ArgumentException("產品價格必須大於0");

   if (quantity < 0) throw new ArgumentException("產品數量不能等於0");

   var product = new Product(_productRepository.GetNextProductId(), name, price, quantity);
   _productRepository.AddProduct(product);
  }
  catch (Exception e)
  {
   Console.WriteLine($"\n錯誤:{e.Message}");
  }
 }

 public void UpdatProduct(Product? product, string? name, decimal price, int quantity)
 {
  try
  {
   if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("產品名稱不能為空");

   if (price <= 0) throw new ArgumentException("產品價格必須大於0");

   if (quantity < 0) throw new ArgumentException("產品數量不能等於0");

   product.Name = name;
   product.Price = price;
   product.Quantity = quantity;
   product.UpdateStatus();
   _productRepository.UpdateProduct(product);
   Console.WriteLine($"產品Id:{product.Id}已更新");
  }
  catch (Exception e)
  {
   Console.WriteLine($"錯誤!更新產品失敗:{e.Message}");
  }
 }
}