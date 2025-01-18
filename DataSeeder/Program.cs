using System;
using System.Collections.Generic;
using System.Linq;
using EstablishmentAPI.Data;
using EstablishmentAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DataSeeder
{
    class Program
    {
        static void Main(string[] args)
        {
            // Создание хоста для использования зависимостей
            var host = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((context, config) =>
                {
                    // Добавление appsettings.json
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    // Регистрация DbContext с использованием строки подключения
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseNpgsql(context.Configuration.GetConnectionString("DefaultConnection")));
                })
                .Build();

            // Создание области сервисов
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AppDbContext>();

                    // Применение миграций, если они еще не применены
                    context.Database.Migrate();

                    // Вызов метода для добавления данных
                    SeedData(context);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }

        static void SeedData(AppDbContext context)
        {
            // Проверка наличия данных, чтобы избежать дублирования
            if (!context.Categories.Any())
            {
                // Добавление категорий
                var categories = new List<Category>
                {
                    new Category { Name = "Пункты питания", Description = "Места для питания" },
                    new Category { Name = "Магазины", Description = "Торговые точки" },
                    new Category { Name = "Развлечения", Description = "Места для отдыха и развлечений" },
                    new Category { Name = "Услуги", Description = "Предоставление различных услуг" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
                Console.WriteLine("Категории добавлены.");
            }

            if (!context.Tags.Any())
            {
                // Добавление тегов
                var tags = new List<Tag>
                {
                    new Tag { Name = "Популярное", Description = "Популярные места" },
                    new Tag { Name = "Новые", Description = "Новые заведения" },
                    new Tag { Name = "Скидки", Description = "Заведения со скидками" },
                    new Tag { Name = "Рекомендовано", Description = "Рекомендуемые места" }
                };

                context.Tags.AddRange(tags);
                context.SaveChanges();
                Console.WriteLine("Теги добавлены.");
            }

            if (!context.Establishments.Any())
            {
                // Получение категорий и тегов для связывания
                var categoryFood = context.Categories.FirstOrDefault(c => c.Name == "Пункты питания");
                var categoryStores = context.Categories.FirstOrDefault(c => c.Name == "Магазины");
                var tagPopular = context.Tags.FirstOrDefault(t => t.Name == "Популярное");
                var tagNew = context.Tags.FirstOrDefault(t => t.Name == "Новые");

                if (categoryFood != null && categoryStores != null && tagPopular != null && tagNew != null)
                {
                    // Добавление заведений
                    var establishments = new List<Establishment>
                    {
                        new Establishment
                        {
                            Name = "Ресторан \"Вкусный обед\"",
                            CategoryId = categoryFood.Id,
                            Address = "ул. Ленина, д. 10",
                            Description = "Популярный ресторан с разнообразным меню",
                            EstablishmentTags = new List<EstablishmentTag>
                            {
                                new EstablishmentTag { TagId = tagPopular.Id },
                                new EstablishmentTag { TagId = tagNew.Id }
                            }
                        },
                        new Establishment
                        {
                            Name = "Супермаркет \"Продукты плюс\"",
                            CategoryId = categoryStores.Id,
                            Address = "пр. Мира, д. 5",
                            Description = "Большой выбор продуктов питания и товаров первой необходимости",
                            EstablishmentTags = new List<EstablishmentTag>
                            {
                                new EstablishmentTag { TagId = tagPopular.Id }
                            }
                        }
                    };

                    context.Establishments.AddRange(establishments);
                    context.SaveChanges();
                    Console.WriteLine("Заведения добавлены.");
                }
                else
                {
                    Console.WriteLine("Не удалось найти необходимые категории или теги для добавления заведений.");
                }
            }

            Console.WriteLine("Данные успешно добавлены.");
        }
    }
}
