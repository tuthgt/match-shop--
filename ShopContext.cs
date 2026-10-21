using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ShopContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
        = null!;

    public DbSet<Product> Products { get; set; }
        = null!;

    public DbSet<Order> Orders { get; set; }
        = null!;

    protected override void OnConfiguring(
        DbContextOptionsBuilder options)
    {
        options.UseNpgsql(
            "Host=localhost;Database=match_shop;Username=postgres;Password=postgres");

        // Вывод SQL-запросов EF Core в консоль.
        options.LogTo(
            Console.WriteLine,
            LogLevel.Information);
    }
}
