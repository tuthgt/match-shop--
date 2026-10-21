using System;
using System.Collections.Generic;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string City { get; set; } = "";

    // Один покупатель может сделать несколько заказов.
    public List<Order> Orders { get; set; } = new();
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int MatchesPerBox { get; set; }
    public int Stock { get; set; }

    // Описание может отсутствовать.
    public string? Description { get; set; }

    // Один товар может встречаться в нескольких заказах.
    public List<Order> Orders { get; set; } = new();
}

public class Order
{
    public int Id { get; set; }
    public int Quantity { get; set; }

    // дата и время создания заказа в UTC.
    public DateTime CreatedAt { get; set; }

    // связь с покупателем.
    public int CustomerId { get; set; }
    public Customer Customer { get; set; } = null!;

    // связь с товаром.
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
}
