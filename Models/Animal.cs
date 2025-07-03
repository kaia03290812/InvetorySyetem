namespace InventorySyetem.Models;

public abstract class Animal
{
 public Animal()
 {
 }

 public Animal(string name)
 {
  Name = name;
 }

 public string Name { get; set; }
 public abstract void MakeSound();
}