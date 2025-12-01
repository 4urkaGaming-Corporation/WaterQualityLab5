Лабораторна робота №5
Тема: RESTful API, версіонування, інтеграційні тести, автогенерація коду (WaterQuality)

========================================
1. Структура проєкту
========================================

WaterQualityLab5/
├─ WaterQualityApi/        - ASP.NET Core Web API (REST, v1/v2, EF Core, SQLite + інші БД)
├─ Client/                 - 2 HTML-сторінки (старий і новий інтерфейс PR5)
└─ Tests/                  - інтеграційні тести на Jest + Supertest

Основний проєкт для Visual Studio: WaterQualityApi (у .sln).

========================================
2. Попередні вимоги
========================================

1) Встановлений .NET SDK (рекомендовано 8.0).
2) Встановлений Node.js (для запуску Jest-тестів).
3) Для автогенерації моделей з БД:
   - глобальний інструмент:
     dotnet tool install --global dotnet-ef

4) SQLite (файл БД waterquality.db у корені WaterQualityApi або свій шлях у appsettings.json).

========================================
3. Налаштування БД (SQLite за замовчуванням)
========================================

Файл: WaterQualityApi/appsettings.json

Розділ:
  "Database": {
    "Provider": "Sqlite",
    "SqlServer": "...",
    "Postgres": "...",
    "MySql": "...",
    "Sqlite": "Data Source=waterquality.db"
  }

За замовчуванням використовується SQLite:
  Provider = "Sqlite"
  ConnectionString = "Data Source=waterquality.db"

Якщо потрібно переключити на іншу БД (для 4-х БД згідно завдання):
  - змінити "Provider" на "SqlServer", "Postgres" або "MySql"
  - вказати коректний connection string.

========================================
4. Запуск API через Visual Studio
========================================

1) Відкрити файл рішення:
   WaterQualityLab5.sln

2) Переконатися, що проєкт за замовчуванням – WaterQualityApi.

3) Запустити (F5 або Ctrl+F5).

4) API буде доступне за адресами:
   - http://localhost:5000
   - https://localhost:5001

5) Перевірити Swagger:
   - https://localhost:5001/swagger
   де будуть дві групи:
   - WaterQuality API v1
   - WaterQuality API v2

========================================
5. Запуск API через командний рядок
========================================

1) Перейти в каталог API:

   cd WaterQualityApi

2) (За потреби) виконати міграції / створення БД.

Якщо використовуєтесь Code First (замість існуючої БД):
   dotnet ef migrations add InitialCreate
   dotnet ef database update

3) Запустити API:

   dotnet run

4) Перевірити роботу:

   GET http://localhost:5000/api/v1/stations
   GET http://localhost:5000/api/v2/measurements?page=1&pageSize=10

========================================
6. Перевірка 2 версій API (v1 і v2)
========================================

v1:
  - GET /api/v1/stations
  - GET /api/v1/stations/{id}
  - POST /api/v1/stations
  - PUT /api/v1/stations/{id}
  - DELETE /api/v1/stations/{id}
  - GET /api/v1/measurements

v2:
  - GET /api/v2/stations?minMeasurements=0
  - GET /api/v2/measurements?stationId=1&page=1&pageSize=20

v1 – стара версія, спрощені моделі.
v2 – нова версія, розширені DTO, фільтри, пагінація.

========================================
7. PR5 інтерфейс (2 сторінки)
========================================

Каталог: Client/

1) index_v1.html – старий інтерфейс:

   - працює з API v1:
     GET http://localhost:5000/api/v1/stations
   - кнопка "Завантажити станції" показує список станцій.

2) index_v2.html – новий інтерфейс:

   - працює з API v2:
     GET http://localhost:5000/api/v2/measurements?...
   - є поля:
     - StationId
     - Page
     - PageSize
   - таблиця з розширеною інформацією (назва станції, параметр, одиниці, час, значення).

Для демонстрації на парі:
  - Запустити API.
  - Відкрити index_v1.html та index_v2.html через браузер (дві вкладки).
  - Показати, що обидві сторінки коректно працюють з поточною версією бекенду (v1 і v2).

========================================
8. Інтеграційні тести (Jest + Supertest)
========================================

Каталог: Tests/

1) Встановлення залежностей (один раз):

   cd Tests
   npm install

2) Перед запуском тестів – API має бути запущене:
   - http://localhost:5000
   або задати змінну середовища API_URL.

3) Запуск тестів (за замовчуванням до localhost:5000):

   cd Tests
   npm test

4) Тести перевіряють:

   - GET /api/v1/stations – повертає 200 та масив.
   - POST /api/v1/stations – створює нову станцію та повертає 201.
   - GET /api/v2/measurements?page=1&pageSize=5 – повертає 200 та масив DTO.

5) Для перевірки роботи з різними БД (завдання – 4 БД):

   1) Змінити "Provider" у WaterQualityApi/appsettings.json:
      - Sqlite
      - SqlServer
      - Postgres
      - MySql

   2) Запустити API з відповідною БД.

   3) Запустити тести:

      cd Tests
      set API_URL=http://localhost:5000   (Windows)
      npm test

   4) Повторити для кожного варіанту БД.

На захисті достатньо показати:
  - що код підтримує всі 4 провайдери,
  - як міняється Provider,
  - як тести виконуються хоча б для одного-двох варіантів у реальному середовищі.

========================================
9. Автогенерація коду (ScaffoldModels)
========================================

У файлі WaterQualityApi/WaterQualityApi.csproj додано ціль MSBuild:

  <Target Name="ScaffoldModels" BeforeTargets="BeforeBuild">
    <Exec Command="dotnet ef dbcontext scaffold &quot;Data Source=waterquality.db&quot; Microsoft.EntityFrameworkCore.Sqlite -o GeneratedModels --force" />
  </Target>

Це означає:
  - перед кожною збіркою проєкту виконується команда:
    dotnet ef dbcontext scaffold "Data Source=waterquality.db" Microsoft.EntityFrameworkCore.Sqlite -o GeneratedModels --force

  - генерується код моделей із поточної схеми БД SQLite у папку:
    WaterQualityApi/GeneratedModels/

  - для кожної таблиці БД створюється окремий .cs-файл (використовує EF Core scaffold).

Для демонстрації на захисті:
  1) Показати вміст csproj з Target ScaffoldModels.
  2) Видалити старий вміст папки GeneratedModels.
  3) Виконати збірку:
     cd WaterQualityApi
     dotnet build
  4) Показати, що папка GeneratedModels знову наповнилась згенерованими файлами.

========================================
10. Основні команди (шпаргалка)
========================================

# Перехід у папку API:
cd WaterQualityApi

# Запуск API:
dotnet run

# Створення міграції (якщо Code First):
dotnet ef migrations add InitialCreate

# Застосування міграції:
dotnet ef database update

# Ручний scaffold з існуючої БД в окрему папку:
dotnet ef dbcontext scaffold "Data Source=waterquality.db" Microsoft.EntityFrameworkCore.Sqlite -o GeneratedModels --force

# Перехід у папку з тестами:
cd Tests

# Встановлення залежностей для тестів:
npm install

# Запуск тестів (API на http://localhost:5000):
npm test

# Встановлення глобального інструменту dotnet-ef (один раз на ПК):
dotnet tool install --global dotnet-ef

========================================
11. Що показати викладачу на захисті
========================================

1) Структуру проєкту (WaterQualityApi, Client, Tests).
2) Swagger з двома версіями API (v1, v2).
3) Роботу двох сторінок інтерфейсу:
   - index_v1.html – стара (v1).
   - index_v2.html – нова (v2).
4) Запуск інтеграційних тестів (Jest).
5) Наявність автогенерації коду (Target ScaffoldModels + папка GeneratedModels).
6) Пояснити, як у appsettings.json можна переключатись між SQLite / SqlServer / Postgres / MySql.
