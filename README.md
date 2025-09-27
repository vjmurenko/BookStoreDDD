# BookStoreDDD

## Описание проекта

BookStoreDDD - это веб-приложение для книжного магазина, реализованное с использованием принципов Domain-Driven Design (DDD). Проект представляет собой модульную архитектуру, разделенную на несколько слоев: доменный, инфраструктурный, веб-интерфейс и т.д. Приложение позволяет пользователям просматривать каталог книг, создавать заказы с различными способами доставки и оплаты.

## Архитектурные особенности

Проект реализован по принципам Domain-Driven Design (DDD) и включает в себя следующие основные компоненты:

- **Store.Domain** 
- **Store.Data.EF** 
- **Store.UI** 
- **Store.Tests** 
- **Store.YandexKasa** 

## Технологии

- **.NET 7.0** 
- **ASP.NET Core MVC**
- **Entity Framework Core 7.0**
- **SQL Server** 
- **xUnit**
- **Moq**
- **Bootstrap** 
- **jQuery** 

## Зависимости

Проект состоит из нескольких проектов:
- Store.Domain 
- Store.Data.EF 
- Store.UI 
- Store.Web.App 
- Store.Tests
- Store.YandexKasa 
- Store.Web.Contractors

## Установка и запуск

1. **Предварительные требования**
   - .NET SDK 7.0 или выше
   - SQL Server (или SQL Server Express)
   - (Опционально) Visual Studio 202 или Visual Studio Code

2. **Клонирование репозитория**
   ```bash
   git clone https://github.com/your-username/BookStoreDDD.git
   cd BookStoreDDD
   ```

3. **Восстановление зависимостей**
   ```bash
   dotnet restore
   ```

4. **Настройка подключения к базе данных**
   
   В файле `Store.UI/appsettings.json` настройте строку подключения к вашей базе данных:
   ```json
   {
     "ConnectionStrings": {
       "storeDb": "Data Source=localhost;Initial Catalog=BookStoreDDD;Integrated Security=true;TrustServerCertificate=true;"
     }
   }
   ```

5. **Применение миграций**
   
   Выполните миграции для создания структуры базы данных:
   ```bash
   cd Store.UI
   dotnet ef database update
   ```

6. **Запуск приложения**
   ```bash
   dotnet run
   ```
   
   Или из Visual Studio запустите проект Store.UI.

7. **Запуск тестов**
   ```bash
   dotnet test
   ```

## Использование

После запуска приложение будет доступно по адресу `https://localhost:5001` или `http://localhost:5000` (в зависимости от настроек SSL).

Основной функционал включает:
- Просмотр каталога книг
- Поиск книг
- Создание заказов
- Выбор способа доставки (включая Postamate)
- Выбор способа оплаты (наличные, Яндекс.Касса)
- Управление корзиной покупок

## Структура проекта

- `/Store.Domain` - доменная модель и бизнес-логика
- `/Store.Data.EF` - реализация репозиториев с Entity Framework
- `/Store.UI` - веб-интерфейс на ASP.NET Core MVC
- `/Store.Web.App` - вспомогательные классы для веб-приложения
- `/Store.Tests` - модульные тесты
- `/Store.YandexKasa` - интеграция с Яндекс.Кассой

## Тестирование

Проект включает модульные тесты, реализованные с использованием xUnit и Moq. Тесты покрывают основную бизнес-логику доменной модели, включая:
- Создание и управление книгами
- Создание и управление заказами
- Управление элементами заказа
- Коллекции элементов заказа

Для запуска тестов выполните команду:
```bash
dotnet test
```
