// 在 StudentManagement.Models 命名空間下底下
using System.ComponentModel.DataAnnotations;

namespace StudentManagement.Models
{
    // 建立 Student 類別，代表一個學生資料。
    public class Student
    {
        // 資料庫內部唯一識別，不顯示給使用者
        // 這是主鍵(Primary Key)，EF Core 會自動辨識 ID 為主鍵
        public int Id { get; set; }

        [Required]
        // 學生的學號
        public int StudentNumber { get; set; } //  自訂的學號欄位

        [Required] // 必填
        // 學生的名字
        public string Name { get; set; }

        [Range(1, 120)] // 年齡範圍
        // 學生的年齡
        public int Age { get; set; }


        [EmailAddress] // 自動驗證 Email 格式
        // 學生的 Email
        public string Email { get; set; }

    }
}
