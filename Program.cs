using Microsoft.EntityFrameworkCore;

Console.OutputEncoding = System.Text.Encoding.UTF8;

using var db = new ShopContext();

// Пересоздаём базу при каждом запуске.
db.Database.EnsureDeleted();
db.Database.EnsureCreated();

Console.WriteLine("Магазин спичек «Чирк»");
Console.WriteLine("База данных создана.\n");

// Шаг 1)

Console.WriteLine("Шаг 1. Покупатели и товары");

Console.WriteLine("Таблицы Customers, Products и Orders созданы.");

// Модели и таблицы определены в Models.cs и ShopContext.cs.

// Шаг 2)

Console.WriteLine("\nШаг 2. Заказы");

Console.WriteLine("Orders связана с Customers и Products.");
Console.WriteLine("В заказе есть два внешних ключа.");

// Шаг 3)

Console.WriteLine("\nШаг 3. Заполнение");

var ivan = new Customer
{
    Name = "Иван Петров",
    Phone = "+7 900 111-22-33",
    City = "Омск"
};

var olga = new Customer
{
    Name = "Ольга Смирнова",
    Phone = "+7 900 444-55-66",
    City = "Томск"
};

var sergey = new Customer
{
    Name = "Сергей Иванов",
    Phone = "+7 900 222-33-44",
    City = "Омск"
};

var anna = new Customer
{
    Name = "Анна Кузнецова",
    Phone = "+7 900 333-44-55",
    City = "Новосибирск"
};

var dmitry = new Customer
{
    Name = "Дмитрий Соколов",
    Phone = "+7 900 555-66-77",
    City = "Томск"
};

db.Customers.AddRange(ivan, olga, sergey, anna, dmitry);

var classic = new Product
{
    Name = "Классические",
    Price = 79m,
    MatchesPerBox = 40,
    Stock = 100,
    Description = "Обычные бытовые спички"
};

var fireplace = new Product
{
    Name = "Каминные",
    Price = 150m,
    MatchesPerBox = 60,
    Stock = 50,
    Description = "Длинные спички для камина"
};

var storm = new Product
{
    Name = "Штормовые",
    Price = 220m,
    MatchesPerBox = 30,
    Stock = 25,
    Description = "Для походов и ветреной погоды"
};

var everyday = new Product
{
    Name = "Повседневные",
    Price = 95m,
    MatchesPerBox = 40,
    Stock = 80,
    Description = null
};

var camping = new Product
{
    Name = "Походные",
    Price = 120m,
    MatchesPerBox = 50,
    Stock = 15,
    Description = "Горят даже под дождём"
};

db.Products.AddRange(
    classic, fireplace, storm, everyday, camping);

// Сохраняем покупателей и товары,
// чтобы база назначила им Id.
db.SaveChanges();

Console.WriteLine("Добавлено покупателей: " +
                  db.Customers.Count());

Console.WriteLine("Добавлено товаров: " +
                  db.Products.Count());

// Четыре заказа. Иван делает два заказа.
// У Дмитрия заказов пока нет.

db.Orders.AddRange(
    new Order
    {
        Customer = ivan,
        Product = fireplace,
        Quantity = 3,
        CreatedAt = DateTime.UtcNow
    },
    new Order
    {
        Customer = ivan,
        Product = classic,
        Quantity = 2,
        CreatedAt = DateTime.UtcNow
    },
    new Order
    {
        Customer = sergey,
        Product = storm,
        Quantity = 1,
        CreatedAt = DateTime.UtcNow
    },
    new Order
    {
        Customer = anna,
        Product = everyday,
        Quantity = 4,
        CreatedAt = DateTime.UtcNow
    }
);

db.SaveChanges();

Console.WriteLine("Добавлено заказов: " +
                  db.Orders.Count());

// Шаг 4)
Console.WriteLine("\nШаг 4. Выборка");

// Зад) Товары дешевле 100 рублей
// от дешёвых к дорогим.

Console.WriteLine("\nТовары дешевле 100 рублей:");

var cheapProducts = db.Products
    .Where(p => p.Price < 100m)
    .OrderBy(p => p.Price)
    .ToList();

foreach (var product in cheapProducts)
{
    Console.WriteLine(
        $"{product.Name} — {product.Price:0.00} ₽");
}

// Зад) Сколько видов товаров есть на складе.

int availableCount = db.Products
    .Count(p => p.Stock > 0);

Console.WriteLine(
    $"\nВидов товаров в наличии: {availableCount}");

// Зад) Три самых дорогих товара.

Console.WriteLine("\nТри самых дорогих товара:");

var expensiveProducts = db.Products
    .OrderByDescending(p => p.Price)
    .Take(3)
    .ToList();

foreach (var product in expensiveProducts)
{
    Console.WriteLine(
        $"{product.Name} — {product.Price:0.00} ₽");
}

// Усложнение. Показываем все заказы.
// Include загружает связанного покупателя и товар.

Console.WriteLine("\nВсе заказы:");

var orders = db.Orders
    .Include(o => o.Customer)
    .Include(o => o.Product)
    .OrderBy(o => o.Id)
    .ToList();

decimal revenue = 0m;

foreach (var order in orders)
{
    decimal sum = order.Product.Price * order.Quantity;
    revenue += sum;

    Console.WriteLine(
        $"{order.Customer.Name} - " +
        $"{order.Product.Name} - " +
        $"{order.Quantity} коробок - " +
        $"{sum:0.00} ₽");
}

Console.WriteLine($"Общая выручка: {revenue:0.00} ₽");

// Шаг 5) 

Console.WriteLine("\nШаг 5. Изменение");

// Пример. Меняем телефон Ивана.

var customer = db.Customers
    .First(c => c.Name == "Иван Петров");

customer.Phone = "+7 900 999-00-00";

Console.WriteLine(
    $"Новый телефон Ивана: {customer.Phone}");

// Зад) Увеличиваем все цены на 10%.

foreach (var product in db.Products)
{
    product.Price *= 1.1m;
}

// Зад) Обнуляем остаток походных спичек.

var outOfStockSoon = db.Products
    .First(p => p.Name == "Походные");

outOfStockSoon.Stock = 0;

db.SaveChanges();

Console.WriteLine("Цены увеличены на 10%.");
Console.WriteLine("Остаток товара «Походные» установлен в 0.");

// Усложнение. Оформление заказа.
// Если товара хватает, создаём заказ и уменьшаем остаток.
// Если товара не хватает, ничего не изменяем.

void PlaceOrder(Customer buyer, Product product, int quantity)
{
    if (quantity <= 0)
    {
        Console.WriteLine("Количество должно быть больше нуля.");
        return;
    }

    if (product.Stock < quantity)
    {
        Console.WriteLine("Недостаточно на складе.");
        return;
    }

    product.Stock -= quantity;

    db.Orders.Add(new Order
    {
        CustomerId = buyer.Id,
        Customer = buyer,
        ProductId = product.Id,
        Product = product,
        Quantity = quantity,
        CreatedAt = DateTime.UtcNow
    });

    // Заказ и остаток сохраняются одним вызовом.
    db.SaveChanges();

    Console.WriteLine(
        $"Заказ оформлен: {buyer.Name}, " +
        $"{product.Name}, {quantity} коробок.");

    Console.WriteLine(
        $"Остаток на складе: {product.Stock} коробок.");
}

var annaFromDb = db.Customers
    .First(c => c.Name == "Анна Кузнецова");

var classicFromDb = db.Products
    .First(p => p.Name == "Классические");

Console.WriteLine("\nПроверка оформления заказа:");

PlaceOrder(annaFromDb, classicFromDb, 2);

// Проверяем случай, когда товара недостаточно.
// Этот вызов не должен изменять базу.

PlaceOrder(annaFromDb, classicFromDb, 10000);

// Шаг 6)

Console.WriteLine("\nШаг 6. Удаление");

// Пример. Удаляем Ольгу.
// У неё нет заказов.

var leaving = db.Customers
    .First(c => c.Name == "Ольга Смирнова");

db.Customers.Remove(leaving);
db.SaveChanges();

Console.WriteLine("Ольга Смирнова удалена.");

// Зад) Удаляем товары с нулевым остатком.

var unavailableProducts = db.Products
    .Where(p => p.Stock == 0)
    .ToList();

db.Products.RemoveRange(unavailableProducts);
db.SaveChanges();

Console.WriteLine(
    $"Удалено товаров без остатка: {unavailableProducts.Count}");

Console.WriteLine(
    $"Товаров осталось: {db.Products.Count()}");


var customerWithOrders = db.Customers
    .Include(c => c.Orders)
    .First(c => c.Name == "Иван Петров");

int deletedOrdersCount = customerWithOrders.Orders.Count;

db.Customers.Remove(customerWithOrders);
db.SaveChanges();

Console.WriteLine(
    $"Удалён Иван Петров и его заказы: {deletedOrdersCount}.");

Console.WriteLine("\nВсе шаги выполнены");
