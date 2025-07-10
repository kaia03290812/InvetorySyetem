// See https://aka.ms/new-console-template for more information

using System.Text;
using InventorySyetem.Models;
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
const string mysqlConnectionString =
 "server=localhost;port=3306;Database=inventory_db;User Id=root;Password=nm5959666";
var connectionString = "";
var configFile = "appsettings.ini";
if (File.Exists(configFile))
{
 Console.WriteLine($"Reading {configFile}file");
 try
 {
  Dictionary<string, Dictionary<string, string>> config = ReadFile(configFile);

  if (config.ContainsKey("Database"))
  {
   Dictionary<string, string> dbConfig = config["Database"];
   connectionString =
    $"Server={dbConfig["Server"]};Port={dbConfig["Port"]};Database={dbConfig["Database"]};uid={dbConfig["Uid"]};pwd={dbConfig["Pwd"]};";
   Console.WriteLine("讀取資料庫連接字串成功！");
  }
 }
 catch (Exception e)
 {
  Console.WriteLine($"讀取設定檔錯誤: {e.Message}");
  connectionString = mysqlConnectionString;
 }
}
else
{
 Console.WriteLine($"錯誤:配置檔案{configFile}不存在");
 connectionString = mysqlConnectionString;
}

Dictionary<string, Dictionary<string, string>> ReadFile(string s)
{
 Dictionary<string, Dictionary<string, string>> config = new(StringComparer.OrdinalIgnoreCase);
 string currentSection = "";

 foreach (string line in File.ReadLines(s))
 {
  string trimmedLine = line.Trim();
  if (trimmedLine.StartsWith("#") || string.IsNullOrWhiteSpace(trimmedLine)) continue; // 跳過註釋和空行

  if (trimmedLine.StartsWith("[") && trimmedLine.EndsWith("]"))
  {
   currentSection = trimmedLine.Substring(1, trimmedLine.Length - 2);
   config[currentSection] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
  }
  else if (currentSection != "" && trimmedLine.Contains("="))
  {
   int equalsIndex = trimmedLine.IndexOf('=');
   string key = trimmedLine.Substring(0, equalsIndex).Trim();
   string value = trimmedLine.Substring(equalsIndex + 1).Trim();
   config[currentSection][key] = value;
  }
 }

 return config;
}

MySqlProductRepository productRepository = new(connectionString);
InventoryService inventoryService = new(productRepository);

EmailNotifier emailNotifier = new();
NotificationService emailService = new(emailNotifier);


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
    SearchProductById();
    break;
   case "3":
    AddProduct();
    break;
   case "4":
    UpdatProduct();
    break;
   case "5":
    SearchProduct();
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
 Console.WriteLine("2. 查詢產品ID");
 Console.WriteLine("3. 新增產品");
 Console.WriteLine("4. 更新產品");
 Console.WriteLine("5. 查詢產品");
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

void SearchProductById()
{
 Console.WriteLine("輸入欲查詢的產品編號");
 var input = ReadIntLine();
 // var product = productRepository.GetProductById(input);
 var product = inventoryService.GetProductById(input);
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
 inventoryService.AddProduct(name, price, quantity);
 // productRepository.AddProduct(name, price, quantity);
 // smsService.NotifyUser("kaia", "新增產品成成功");
}

void SearchProduct()
{
 Console.WriteLine("輸入欲查詢的產品關鍵字");
 string input = Console.ReadLine();
 List<Product> product = inventoryService.SearchProduct(input);
 if (product.Any())
 {
  Console.WriteLine($"----------查詢條件為:{input}-------------------");
  Console.WriteLine("ID | Name | Price | Quantity | Status");
  Console.WriteLine("-----------------------------------------------");
  foreach (Product p in product) Console.WriteLine($"{p.Id} | {p.Name} | {p.Price} | {p.Quantity} | {p.Status}");

  Console.WriteLine("-----------------------------------------------");
 }
 else
 {
  Console.WriteLine("查無此產品");
 }
}

void UpdatProduct()
{
 Console.WriteLine("請輸入要更新的產品編號：");
 var id = ReadIntLine();
 var product = inventoryService.GetProductById(id);

 if (product == null)
 {
  Console.WriteLine("查無此產品！");
  return;
 }

 Console.WriteLine("輸入產品名稱：");
 var name = Console.ReadLine();

 Console.WriteLine("輸入產品價格：");
 var price = ReadDecimalLine();

 Console.WriteLine("輸入產品數量：");
 var quantity = ReadIntLine();

 inventoryService.UpdatProduct(product, name, price, quantity);
 Console.WriteLine("產品更新成功！");
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