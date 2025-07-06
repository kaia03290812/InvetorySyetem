using InventorySyetem.Models;
using InventorySyetem1.Repositories;
using MySql.Data.MySqlClient;

namespace InventorySyetem.Repositories;

public class MySqlProductRepository : IProductRepository
{
 private readonly string _connectionString;

//constructor
 public MySqlProductRepository(string connectionString)
 {
  _connectionString = connectionString;
  InitializeDatabase();
 }

 public Product.ProductStatus Status { get; set; }

 public List<Product> GetAllProducts()
 {
  var products = new List<Product>();
  using (var connection = new MySqlConnection(_connectionString))
  {
   connection.Open();
   var selectsql = "SELECT * FROM products";
   using (var cmd = new MySqlCommand(selectsql, connection))
   {
    using (var reader = cmd.ExecuteReader())
    {
     while (reader.Read())
     {
      var product = new Product(
       reader.GetInt64("id"),
       reader.GetString("name"),
       reader.GetDecimal("price"),
       reader.GetInt32("quantity")
      );
      product.Status = (Product.ProductStatus)reader.GetInt32("status");
      products.Add(product);
     }
    }
   }
  }

  return products;
 }

 public Product? GetProductById(int id)
 {
  Product product = null;
  using (var connection = new MySqlConnection(_connectionString))
  {
   connection.Open();
   var selectsql = "SELECT * FROM products WHERE id = @id";
   using (var cmd = new MySqlCommand(selectsql, connection))
   {
    cmd.Parameters.AddWithValue("@id", id);
    using (var reader = cmd.ExecuteReader())
    {
     if (reader.Read())
     {
      product = new Product(reader.GetInt64("id"),
       reader.GetString("name"),
       reader.GetDecimal("price"),
       reader.GetInt32("quantity"));
      product.Status = (Product.ProductStatus)reader.GetInt32("status");
     }
    }
   }
  }

  return product;
 }

 public void AddProduct(Product product)
 {
  using (var connection = new MySqlConnection(_connectionString))
  {
   connection.Open();
   var insertSql = @"INSERT INTO products (name, price, quantity, status) 
                                    values (@name, @price, @quantity, @status)";
   using (var cmd = new MySqlCommand(insertSql, connection))
   {
    cmd.Parameters.AddWithValue("@name", product.Name);
    cmd.Parameters.AddWithValue("@price", product.Price);
    cmd.Parameters.AddWithValue("@quantity", product.Quantity);
    cmd.Parameters.AddWithValue("@status", product.Status);
    cmd.ExecuteNonQuery();
   }
  }
 }

 public void UpdateProduct(Product product)
 {
  using (var connection = new MySqlConnection(_connectionString))
  {
   connection.Open();
   var insertSql = @"UPDATE products SET name=@name,price=@price,quantity=@quantity, status= @status
WHERE id=@id";
   using (var cmd = new MySqlCommand(insertSql, connection))
   {
    cmd.Parameters.AddWithValue("@id", product.Id);
    cmd.Parameters.AddWithValue("@name", product.Name);
    cmd.Parameters.AddWithValue("@price", product.Price);
    cmd.Parameters.AddWithValue("@quantity", product.Quantity);
    cmd.Parameters.AddWithValue("@status", product.Status);
    cmd.ExecuteNonQuery();
   }
  }
 }

 public int GetNextProductId()
 {
  using (var connection = new MySqlConnection(_connectionString))
  {
   connection.Open();
   var selectaql = @"SELECT IFNULL(MAX(id),0) FROM products";
   using (var cmd = new MySqlCommand(selectaql, connection))
   {
    var result = Convert.ToInt64(cmd.ExecuteScalar());
    if (result != 0) return (int)(result + 1);

    return 0;
   }
  }
 }

 private void InitializeDatabase()
 {
  using (var connection = new MySqlConnection(_connectionString))
  {
   try
   {
    connection.Open();
    string createTableSql = @"
    CREATE TABLE IF NOT EXISTS products( 
        id INT PRIMARY KEY AUTO_INCREMENT,
        name VARCHAR(100) NOT NULL,
        price DECIMAL(10,2) NOT NULL,
        quantity INT NOT NULL,
        status INT NOT NULL
    );";

    using (var cmd = new MySqlCommand(createTableSql, connection))
    {
     cmd.ExecuteNonQuery();
    }

    Console.WriteLine("MySQL 初始化成功或表已存在");
   }
   catch (MySqlException e)
   {
    Console.WriteLine($"初始化 MySQL 失敗: {e.Message}");
   }
  }
 }
}