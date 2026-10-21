using Microsoft.EntityFrameworkCore;

Console.WriteLine("Магазин спичек «Чирк»: проект запущен!");

// Дальше — код по шагам задания.

using var db = new ShopContext();

db.Database.EnsureDeleted();
db.Database.EnsureCreated();
