# 💊 Pharmacy Management System

Това е уеб базирано приложение за управление на аптека, изградено със софтуерната архитектура **ASP.NET Core MVC**. Проектът осигурява управление на инвентар, проследяване на рецепти и персонална статистика за фармацевти.

## 🚀 Технологии
* **Backend:** .NET 8.0 / 9.0 (ASP.NET Core MVC)
* **Database:** Entity Framework Core (SQLite)
* **Security:** ASP.NET Core Identity (Role-based Authorization)
* **Frontend:** Razor Views, Bootstrap 5, Bootstrap Icons
* **Validation:** Client-side (jQuery) & Server-side (Data Annotations)

## 🎯 Основни функционалности
1. **Inventory Management:** Пълен CRUD за лекарства (Medicaments).
2. **Prescription Tracking:** Издаване на рецепти, автоматично свързани с логнатия потребител.
3. **Smart Alerts:** Автоматично филтриране на лекарства с ниска наличност (под 10 броя) и изтекъл срок на годност.
4. **Personal Dashboard:** Индивидуална статистика за всеки фармацевт (топ продажби и средна стойност на рецепта).
5. **Role Security:** Разделение на правата между `Admin` и `Pharmacist`.

---

## 🛠 Инструкции за стартиране

Изпълнете следните команди в терминала на вашия проект:

### 1. Инсталиране на зависимостите
```bash
dotnet restore
```

### 2. Подготовка на базата данни
```bash
dotnet ef database update
```

### 3. Стартиране на приложението
```bash
dotnet run
``` 
