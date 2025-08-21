
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//告訴程式你要用 ASP.NET Core MVC 的功能，MVC 裡面的 Controller、ActionResult、View() 都在這裡面


using StudentManagement.Data;
//引入我們剛剛建立的 AppDbContext，讓 Controller 可以連線資料庫



using StudentManagement.Models;
//讓我們可以使用 async/await，例如：非同步存資料到資料庫



using System.Linq;
//引入我們的 Student 類別，讓 Controller 知道 Student 這個型別



using System.Threading.Tasks;
//提供查詢功能，例如 ToList()、Where()，方便我們從資料庫取出資料



namespace StudentManagement.Controllers
{
    // StudentsController 處理學生相關的網頁請求
    public class StudentsController : Controller
    {
        // 宣告一個私有變數 _context，用來操作資料庫
        private readonly AppDbContext _Context;

        // 建構子，透過依賴注入 (Dependency Injection) 拿到資料庫上下文
        public StudentsController(AppDbContext context)
        {
            _Context = context; // _context = context; → 存起來方便整個 Controller 使用
        }


        // GET : /Students
        // 顯示所有學生資料
        public IActionResult Index() //對應網址 /Students 或 /Students/Index
        {
            // 從資料庫取出所有學生資料，傳到 View
            var students = _Context.Students.ToList();
            return View(students);
        }

        // GET : /Students/Create
        // 顯示新增學生表單
        public IActionResult Create() //使用者要新增學生時，先顯示一個空白表單
        {
            return View();
        }

        // POST : /Students/Creste
        // 接收使用者提交的新增學生表單

        [HttpPost] //表示這個方法處理 POST 請求（表單送出）

        [ValidateAntiForgeryToken] // 防止 CSRF 攻擊
        public async Task<IActionResult> Create(Student student)
        //async Task<IActionResult> → 非同步方法，方便存資料庫時不阻塞伺服器
        //Student student → ASP.NET Core 自動把表單資料轉成 Student 物件

        {
            // 檢查模型驗證是否成功
            if (ModelState.IsValid) {


                // 找出現有的最大學號
                int maxStudentNumber = _Context.Students.Any()
                    ? _Context.Students.Max(s => s.StudentNumber) : 0;

                // 新學生的學號 = 最大值 + 1
                student.StudentNumber = maxStudentNumber + 1;

                _Context.Students.Add(student);
                await _Context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }


            return View(student);// 如果資料有錯，重新顯示表單
        }


        // GET: /Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 找到要編輯的學生
            var student = await _Context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }



        // POST: /Students/Edit/5
        [HttpPost] // 表示這個方法處理 POST 請求（表單送出）
        [ValidateAntiForgeryToken] // 防止 CSRF 攻擊，確保表單是本站發出的
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Age,Email")] Student student)
        {
            // 檢查網址 id 與表單傳來的學生 Id 是否一致
            // 如果不一致，表示可能有人在網址亂改，回傳 404
            if (id != student.Id)
            {
                return NotFound();
            }

            //  檢查模型驗證是否成功（例如 Name 必填、Email 格式等）
            if (ModelState.IsValid)
            {
                try
                {
                    // 從資料庫抓出原本學生資料
                    // 使用 FindAsync 非同步取得資料
                    var studentToUpdate = await _Context.Students.FindAsync(id);

                    //  如果找不到學生資料，回傳 404
                    if (studentToUpdate == null)
                    {
                        return NotFound();
                    }

                    // 更新欄位
                    // 手動將表單的 Name、Age、Email 資料更新到原本資料物件
                    studentToUpdate.Name = student.Name;
                    studentToUpdate.Age = student.Age;
                    studentToUpdate.Email = student.Email;

                    // 注意：StudentNumber 沒有被覆蓋，保留原本值

                    //  儲存變更到資料庫
                    await _Context.SaveChangesAsync();

                    //  更新完成後，導向學生列表頁
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    // 捕捉資料庫同時更新衝突的例外
                    // 例如同一筆資料被其他使用者刪除或更新
                    if (!_Context.Students.Any(e => e.Id == student.Id))
                    {
                        // 如果資料已不存在，回傳 404
                        return NotFound();
                    }
                    else
                    {
                        // 其他異常直接丟出
                        throw;
                    }
                }
            }

            // 若資料驗證失敗（例如欄位空白、格式錯誤）
            // 重新顯示表單並帶回使用者輸入的資料與錯誤訊息
            return View(student);
        }





        // GET : /Students/Delete/5
        // 顯示刪除確認頁面
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // 從資料庫找到要刪除的學生
            var student = await _Context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student); // 把學生資料傳到刪除確認頁面
        }

        // 處理 POST 請求，刪除學生資料
        // ActionName("Delete") → 讓路由還是用 /Delete/5 來呼叫這個方法
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken] // 防止 CSRF 攻擊
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // 從資料庫中找到要刪除的學生
            var student = _Context.Students.Find(id);

            // 如果找到了這個學生才進行刪除
            if (student != null)
            {
                // 記錄被刪除學生的學號，用於後續調整其他學生的學號
                int deletedNumber = student.StudentNumber;

                // 將該學生從資料庫暫存中移除
                _Context.Students.Remove(student);

                // 將刪除動作存回資料庫
                _Context.SaveChanges();

                // ⚡ 調整其他學生的學號：
                // 找出所有學號大於被刪除學生的學生
                var studentsToUpdate = _Context.Students
                    .Where(s => s.StudentNumber > deletedNumber) // 篩選學號比刪除的學生大的
                    .ToList(); // 轉成 List 方便迴圈處理

                // 將這些學生的學號全部減 1
                foreach (var s in studentsToUpdate)
                {
                    s.StudentNumber -= 1;
                }

                // 儲存修改後的學號回資料庫
                _Context.SaveChanges();
            }

            // 刪除完成後，重新導向學生列表頁面
            return RedirectToAction(nameof(Index));
        }


    }
}