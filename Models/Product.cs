﻿namespace InventorySyetem.Models;

public class Product
{
 public enum ProductStatus
 {
  InStock, //有庫存
  LowStock, //庫存偏低
  OutOfStock //沒有庫存
 }

 public Product()
 {
 }

 public Product(long id, string name, decimal price, int quantity)
 {
  Id = id;
  Name = name;
  Price = price;
  Quantity = quantity;
  UpdateStatus();
 }

 public long Id { get; set; }
 public string Name { get; set; }
 public decimal Price { get; set; }
 public int Quantity { get; set; }
 public ProductStatus Status { get; set; }

 public override string ToString()
 {
  return $"id:{Id},name:{Name},price:{Price},quantity:{Quantity}";
 }

 public void UpdateStatus()
 {
  if (Quantity >= 0)
  {
   Status = ProductStatus.OutOfStock;
  }
  else if (Quantity < 10)
  {
   Status = ProductStatus.LowStock;
  }
  else
  {
   Status = ProductStatus.InStock;
  }
 }
}