// See https://aka.ms/new-console-template for more information

using System.Text;
using InventorySyetem.Repositories;
using InventorySyetem.Services;
using InventorySyetem.Utils;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;
//Server: mysql所在伺服器位置（localhost or ip xxx.xxx.xxx.xxx）
//Port: mysql端口（預設3306）
//Database: inventory_db(CREATE DATABASE inventory_db;)
//uid: mysql使用者名稱
//pwd: mysql使用者密碼
const string MYSQL_CONNECTION_STRING =
 "server=localhost;port=3306;Database=inventory_db;User Id=root;Password=nm5959666";
var productRepository = new MySqlProductRepository(MYSQL_CONNECTION_STRING);
var inventoryService = new InventoryService(productRepository);

var emailNotifier = new EmailNotifier();
var emailService = new NotificationService(emailNotifier);

var smsNotifier = new SmsNotifier();
var smsService = new NotificationService(smsNotifier);

RunMenu();

void RunMenu()
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

void DisplayMenu()
{
 Console.WriteLine("Welcome to the inventory system!");
 Console.WriteLine("What would you like to do?");
 Console.WriteLine("1. 查看所有產品");
 Console.WriteLine("2. 查詢產品");
 Console.WriteLine("3. 新增產品");
 Console.WriteLine("0. 離開");
}

void GetAllProducts()
{
 Console.WriteLine("\n--- 所有產品列表 ---");
 var products = productRepository.GetAllProducts();
 if (products.Any())
 {
  Console.WriteLine("-----------------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("-----------------------------------------------");
  foreach (var product in products)
  {
   Console.WriteLine(product);
  }

  Console.WriteLine("-----------------------------------------------");
  emailService.NotifyUser("kaia", "查詢已完成");
 }
}

void SearchProduct()
{
 Console.WriteLine("輸入欲查詢的產品編號");
 var input = ReadIntLine();
 var product = productRepository.GetProductById(input);

 if (product != null)
 {
  Console.WriteLine("-----------------------------------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("-----------------------------------------------");
  Console.WriteLine(product);
  Console.WriteLine("-----------------------------------------------");
 }
 else
 {
  Console.WriteLine("查無此產品");
 }
}

void AddProduct()
{
 Console.WriteLine("輸入產品名稱：");
 var name = Console.ReadLine();
 Console.WriteLine("輸入產品價格：");
 var price = ReadDecimalLine();
 Console.WriteLine("輸入產品數量：");
 var quantity = ReadIntLine();
 productRepository.AddProduct(name, price, quantity);
 smsService.NotifyUser("kaia", "新增產品成成功");
}

int ReadInt(string input)
{
 try
 {
  return Convert.ToInt32(input);
 }
 catch (FormatException e)
 {
  Console.WriteLine("請輸入有效數字。");
  return 0;
 }

 smsNotifier.SendNotification("", "");
}

int ReadIntLine(int defaultValue = 0)
{
 while (true)
 {
  var input = Console.ReadLine();
  if (string.IsNullOrWhiteSpace(input) && defaultValue != 0) return defaultValue;

  //string parsing to int 
  if (int.TryParse(input, out var value))
   return value;
  else
   Console.WriteLine("請輸入有效數字。");
 }
}

decimal ReadDecimalLine(decimal defaultValue = 0.0m)
{
 while (true)
 {
  var input = Console.ReadLine();
  if (string.IsNullOrWhiteSpace(input) && defaultValue != 0.0m) return defaultValue;

  //string parsing to int 
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