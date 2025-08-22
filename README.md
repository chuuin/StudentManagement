# 學生資料管理系統 (Student Management)

一個使用 **ASP.NET Core MVC** 與 **Entity Framework Core** 建立的基礎專案，示範如何進行 CRUD 與資料庫連線。  
此專案主要功能為 **學生資料管理**，提供學生的新增、編輯、刪除與查詢。

---

##  專案功能
-  學生列表顯示 (Index)
-  新增學生 (Create)
-  編輯學生 (Edit)
-  刪除學生 (Delete)


---

##  資料庫設計 (MSSQL)
資料表名稱：`Students`

| 欄位  | 型別        | 描述      |
|-------|-------------|-----------|
| Id    | int (PK)    | 學生編號  |
| Name  | nvarchar(50)| 學生姓名  |
| Age   | int         | 年齡      |
| Email | nvarchar(50)| 電子郵件  |

---

##  技術
- ASP.NET Core MVC
- Razor Pages
- Entity Framework Core
- MSSQL (SQL Server)

---

##  開始使用
1. **建立資料庫**
   ```sql
   CREATE DATABASE StudentDB;
