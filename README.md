# EstablishmentAPI

## Описание

**EstablishmentAPI** — это RESTful API, разработанный на платформе **.NET 8**, предназначенный для управления категориями и заведениями. Проект обеспечивает полный набор операций CRUD (создание, чтение, обновление, удаление) для категорий и заведений, а также управление тегами, связанными с заведениями. API использует **Entity Framework Core** для взаимодействия с базой данных и **AutoMapper** для преобразования данных между объектами моделей и DTO (Data Transfer Objects).

### Основные возможности:

- **Управление категориями заведений**: создание, получение, обновление и удаление категорий.
- **Управление заведениями**: создание, получение, обновление и удаление заведений.
- **Управление тегами**: создание, получение, обновление и удаление тегов, связанных с заведениями.
- **Валидация данных**: проверка входящих данных на соответствие бизнес-логике и требованиям.
- **Автоматическая генерация миграций**: упрощение процесса обновления структуры базы данных.
- **Документация API**: интерактивная документация с использованием Swagger для удобного тестирования эндпоинтов.
- **Логирование**: встроенные механизмы логирования для отслеживания работы приложения и диагностики ошибок.

## Технологии

- **.NET 8** — современная платформа для разработки приложений.
- **ASP.NET Core** — фреймворк для создания веб-приложений и API.
- **Entity Framework Core** — ORM для взаимодействия с базой данных.
- **AutoMapper** — библиотека для автоматического маппинга объектов.
- **Swagger (Swashbuckle)** — инструмент для документирования и тестирования API.
- **SQL Server** — система управления базами данных (может быть заменена на другую при необходимости).

## Требования

- **.NET 8 SDK** — [Скачать .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0)
- **SQL Server** — [Скачать SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (или использовать другой провайдер базы данных, поддерживаемый Entity Framework Core)
- **Visual Studio 2022** или **Visual Studio Code** — для разработки и отладки приложения
- **Git** — для управления версиями и клонирования репозитория

## Настройка

### 1. Настройка Строки Подключения к Базе Данных
Откройте файл appsettings.json и настройте строку подключения к вашей базе данных:
```
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=EstablishmentDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```
### 2. Применение Миграций Базы Данных
Перед запуском приложения необходимо создать и применить миграции для инициализации базы данных:
```
dotnet ef migrations add InitialCreate
dotnet ef database update
```
### Структура проекта
```
EstablishmentAPI/
├── Controllers/
│   ├── CategoriesController.cs
│   ├── EstablishmentsController.cs
│   └── TagsController.cs
├── DTOs/
│   ├── CategoryDTO.cs
│   ├── CreateCategoryDTO.cs
│   ├── EstablishmentDTO.cs
│   ├── CreateEstablishmentDTO.cs
│   ├── TagDTO.cs
│   └── CreateTagDTO.cs
├── Models/
│   ├── Category.cs
│   ├── Establishment.cs
│   └── Tag.cs
├── Data/
│   └── AppDbContext.cs
├── Profiles/
│   └── MappingProfile.cs
├── Migrations/
│   ├── {timestamp}_InitialCreate.cs
│   └── {timestamp}_InitialCreate.Designer.cs
├── Tests/
│   ├── CategoriesControllerTests.cs
│   └── EstablishmentsControllerTests.cs
├── appsettings.json
├── Program.cs
└── README.md
```
#### Краткое Описание Компонентов
Controllers/ — содержит контроллеры для управления категориями, заведениями и тегами.
DTOs/ — содержит классы DTO для передачи данных между клиентом и сервером.
Models/ — содержит классы моделей данных, соответствующие структуре базы данных.
Data/ — содержит класс контекста базы данных AppDbContext.
Profiles/ — содержит профиль AutoMapper для настройки маппингов между DTO и моделями.
Migrations/ — содержит файлы миграций Entity Framework Core.
Tests/ — содержит юнит-тесты и интеграционные тесты для контроллеров и других компонентов.
appsettings.json — файл конфигурации приложения, включая строки подключения.
Program.cs — основной файл запуска приложения.
README.md — текущий файл с описанием проекта.
