// Классы таблиц: Customer, Product, Order (шаги 1–2).

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Phone { get; set; } = "";
    public string City { get; set; } = "";
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
    public int MatchesPerBox { get; set; }
    public int Stock { get; set; }

    public string? Description { get; set; }
}
