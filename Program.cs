using System.Text;
using InventorySyetem.Repositories;
using InventorySyetem.Services;
using InventorySyetem.Utils;

internal class Program
{
 private static object emailService;
 private static MySqlProductRepository productRepository;

 private static void Main(string[] args)
 {
  Console.OutputEncoding = Encoding.UTF8;
  Console.InputEncoding = Encoding.UTF8;

  const string MYSQL_CONNECTION_STRING =
   "server=localhost;port=3306;Database=inventory_db;User Id=root;Password=nm5959666";
  productRepository = new MySqlProductRepository(MYSQL_CONNECTION_STRING);
  var inventoryService = new InventoryService(productRepository);

  var emailNotifier = new EmailNotifier();
  var emailService = new NotificationService(emailNotifier);


  RunMenu();
  return;
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
    case "3":
     AddProduct();
     break;
    case "0":
     Console.WriteLine("Goodbye !");
     return;
    default:
     Console.WriteLine("請輸入有效的選項（0~3）");
     break;
   }
  }
 }

 private static void AddProduct()
 {
  Console.WriteLine("請輸入產品名稱");
  var name = Console.ReadLine();

  Console.WriteLine("輸入產品價格:");
  var price = ReadDecimalLine();

  Console.WriteLine("輸入產品數量");
  var quantity = ReadIntLine();

  productRepository.AddProduct(name, price, quantity);
 }


 private static decimal ReadDecimalLine(decimal defaultValue = 0.0m)
 {
  while (true)
  {
   var input = Console.ReadLine();

   if (string.IsNullOrWhiteSpace(input) && defaultValue != 0.0m) return defaultValue;

   if (decimal.TryParse(input, out var value))
   {
    return value;
   }
   else
   {
    Console.WriteLine("請輸入有效數字。");
   }
  }
 }

 private static void DisplayMenu()
 {
  Console.WriteLine("Welcome to the inventory system!");
  Console.WriteLine("What would you like to do?");
  Console.WriteLine("1. 查看所有商品");
  Console.WriteLine("2. 查詢產品");
  Console.WriteLine("3. 新增產品");
  Console.WriteLine("0. 離開");
 }

 private static void GetAllProducts()
 {
  Console.WriteLine("\n--- 所有產品列表 ---");
  var products = InventoryService.GetAllProducts();
  Console.WriteLine("--------------------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("--------------------------------------------------");
  foreach (var product in products)
  {
   Console.WriteLine(products);
  }

  Console.WriteLine("--------------------------------------------------");

  // emailService.NotifyUser("kaia", "查詢已完成");
 }


 private static void SearchProduct()
 {
  Console.WriteLine("請輸入欲查詢的產品編號");
  var input = ReadIntLine();

  var product = productRepository.GetProductById(input);

  if (product != null)
  {
   Console.WriteLine("-----------------------------");
   Console.WriteLine("ID | Name | Price | Quantity | Status");
   Console.WriteLine("------------------------------------------------");
   Console.WriteLine(product);
   Console.WriteLine("------------------------------------------------");
  }
  else
  {
   Console.WriteLine("查無此產品");
  }
 }

 private static int ReadIntLine(int defaultValue = 0)
 {
  while (true)
  {
   var input = Console.ReadLine();

   if (string.IsNullOrWhiteSpace(input) && defaultValue != 0) return defaultValue;

   if (int.TryParse(input, out var result))
    return result;
   else
    Console.WriteLine("請輸入有效數字");
  }
 }
}