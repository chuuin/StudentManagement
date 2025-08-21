// 匯入 EF Core 和 Models
/* 這是 Entity Framework Core (EF Core) 的核心命名空間。

using Microsoft.EntityFrameworkCore;

EF Core 是微軟提供的一個 ORM（Object-Relational Mapping）框架，簡單說：

讓你用 C# 類別 (Class) 去操作 資料庫表格 (Table)

不用寫 SQL 就能做新增、修改、刪除、查詢等操作*/
using Microsoft.EntityFrameworkCore;
using StudentManagement.Models;


namespace StudentManagement.Data
{
    // AppDbContext 負責資料庫連線與資料表管理
    
    public class AppDbContext : DbContext
    /* public class AppDbContext → 定義一個叫 AppDbContext 的類別

: DbContext → 表示這個類別 繼承 自 EF Core 的 DbContext

DbContext 是 EF Core 的核心類別，負責：

連接資料庫

管理資料表

提供 CRUD (新增、查詢、修改、刪除) 功能

簡單比喻：


EF Core 是「工具箱」

DbContext 是「操作資料庫的工具」

AppDbContext 就是你專案裡的工具，專門管理學生資料表。 */
    {
        // 建構子，接收選項
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        /*建構子 (Constructor)：當你創建 AppDbContext 物件時會執行

DbContextOptions<AppDbContext>：

用來告訴 EF Core 你的資料庫連線字串、要使用哪種資料庫（SQL Server / SQLite 等）

: base(options)：

把這個選項交給 DbContext 的父類別

讓 EF Core 知道「我應該連哪個資料庫」*/
        {
        }

        // Students 資料表對應 Student Model
        public DbSet<Student> Students { get; set; }
        /*AppDbContext 是「資料庫管家」

DbSet<Student> 是「學生資料櫃子」

你可以從這個櫃子拿資料、放資料、改資料。*/
    }
}
