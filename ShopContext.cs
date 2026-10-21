// Контекст базы ShopContext (шаг 1).

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

public class ShopContext : DbContext
{
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(
        DbContextOptionsBuilder options)
    {
        options.UseNpgsql(
            "Host=localhost;Database=match_shop;Username=postgres;Password=postgres");

        options.LogTo(
            Console.WriteLine,
            LogLevel.Information);
    }
}
