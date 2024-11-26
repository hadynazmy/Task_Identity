using Microsoft.AspNetCore.Authorization; // استيراد المكون الخاص بالتحقق من الوصول باستخدام الأذونات.
using Microsoft.AspNetCore.Identity; // استيراد المكون الخاص بإدارة الهوية (Identity) في ASP.NET Core.
using Microsoft.AspNetCore.Mvc; // استيراد المكون الخاص بتطوير تطبيقات الويب باستخدام MVC.
using Microsoft.EntityFrameworkCore; // استيراد المكون الخاص بالتعامل مع قواعد البيانات باستخدام Entity Framework Core.
using TaskEngAmr.ViewModels; // استيراد مساحة الأسماء التي تحتوي على النماذج (ViewModels) الخاصة بالتطبيق.

namespace TaskEngAmr.Controllers
{
    // تطبيق سياسة الصلاحيات: هذا الـ Controller يتطلب أن يكون المستخدم في دور "Admin" للوصول إلى الإجراءات.
    [Authorize(Roles = "Admin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager; // حقل لتخزين كائن مدير الأدوار (Role Manager).

        // المُنشئ: يتم تمرير كائن RoleManager عبر الاعتماديات.
        public RolesController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager; // تخزين الكائن في الحقل الخاص.
        }

        // الإجراء الخاص بعرض قائمة الأدوار.
        public async Task<IActionResult> Index()
        {
            // استرجاع قائمة الأدوار الموجودة في قاعدة البيانات باستخدام RoleManager.
            var roles = await _roleManager.Roles.ToListAsync();

            // تمرير قائمة الأدوار إلى العرض (View) لعرضها.
            return View(roles);
        }

        // إجراء لإضافة دور جديد إلى النظام. يستخدم HTTP POST.
        [HttpPost]
        [ValidateAntiForgeryToken] // حماية ضد هجمات تزوير الطلبات (CSRF).
        public async Task<IActionResult> Add(RoleFormViewModel model)
        {
            // إذا كان النموذج غير صالح (بسبب فشل التحقق من البيانات).
            if (!ModelState.IsValid)
            {
                // إعادة عرض صفحة الأدوار مع البيانات الحالية.
                return View("Index", await _roleManager.Roles.ToListAsync());
            }

            // التحقق مما إذا كان الدور موجودًا بالفعل في قاعدة البيانات.
            if (await _roleManager.RoleExistsAsync(model.Name))
            {
                // إضافة رسالة خطأ إلى النموذج.
                ModelState.AddModelError("Name", "Role is exists!");

                // إعادة عرض صفحة الأدوار مع البيانات الحالية والأخطاء.
                return View("Index", await _roleManager.Roles.ToListAsync());
            }

            // إنشاء دور جديد باستخدام اسم الدور المدخل من النموذج.
            await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));

            // إعادة توجيه المستخدم إلى الإجراء Index بعد إضافة الدور بنجاح.
            return RedirectToAction(nameof(Index));
        }
    }
}
