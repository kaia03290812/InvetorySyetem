// See https://aka.ms/new-console-template for more information

using System.Text;
using InventorySyetem.Repositories;

internal class Program
{
 private static MySqlProductRepository productRepository;

 private static void Main(string[] args)
 {
  Console.OutputEncoding = Encoding.UTF8;
  const string MYSQL_CONNECTION_STRING =
   "server=localhost;port=3306;Database=inventory_db;User Id=root;Password=nm5959666";
  productRepository = new MySqlProductRepository(MYSQL_CONNECTION_STRING);

  RunMenu();
 }

 private static void RunMenu()
 {
  while (true)
  {
   DisplayMenu();
   var input = Console.ReadLine();
   switch (input)
   {
    case "1":
     GetAllProducts();
     break;
    case "2":
     SearchProduct();
     break;
    case "0":
     Console.WriteLine("Goodbye !");
     return;
   }
  }
 }

 private static void DisplayMenu()
 {
  Console.WriteLine("Welcome to the inventory system!");
  Console.WriteLine("What would you like to do?");
  Console.WriteLine("1. 查看所有商品");
  Console.WriteLine("2. 查詢產品");
  Console.WriteLine("3. 離開");
 }

 private static void GetAllProducts()
 {
  Console.WriteLine("\n--- 所有產品列表 ---");
  var products = productRepository.GetAllProducts();
  if (products.Any())
  {
   Console.WriteLine("--------------------------------------------------");
   Console.WriteLine("ID | Name | Price | Quantity | Status");
   Console.WriteLine("--------------------------------------------------");
   foreach (var product in products) Console.WriteLine(product);

   Console.WriteLine("--------------------------------------------------");
  }
 }

 private static void SearchProduct()
 {
  Console.WriteLine("請輸入欲查詢的產品編號");
  var input = Console.ReadLine();
  var product = productRepository.GetProductById(input);


  if (product != null)
  {
   Console.WriteLine("-----------------------------");
   Console.WriteLine("ID | Name | Price | Quantity | Status");
   Console.WriteLine("------------------------------------------------");
   Console.WriteLine(product);
   Console.WriteLine("------------------------------------------------");
  }
 }
}