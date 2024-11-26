// مرخصة إلى مؤسسة .NET Foundation بموجب اتفاقيات متعددة.
// مؤسسة .NET ترخص هذا الملف بموجب رخصة MIT.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using TaskEngAmr.Models;

namespace TaskEngAmr.Areas.Identity.Pages.Account
{
    // هذا هو النموذج المسؤول عن تسجيل المستخدمين في النظام.
    public class RegisterModel : PageModel
    {
        // تعريف الخصائص اللازمة لإدارة تسجيل الدخول والمستخدمين وإرسال البريد الإلكتروني وتسجيل الأحداث.
        private readonly SignInManager<ApplicationUser> _signInManager; // مسؤول عن إدارة تسجيل الدخول.
        private readonly UserManager<ApplicationUser> _userManager; // مسؤول عن إدارة المستخدمين.
        private readonly IUserStore<ApplicationUser> _userStore; // مسؤول عن تخزين بيانات المستخدم.
        private readonly IUserEmailStore<ApplicationUser> _emailStore; // مسؤول عن تخزين البريد الإلكتروني.
        private readonly ILogger<RegisterModel> _logger; // لتسجيل الأحداث.
        private readonly IEmailSender _emailSender; // لإرسال رسائل البريد الإلكتروني.

        // المُنشئ لتلقي الكائنات الضرورية من خلال حقن التبعيات.
        public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager; // تهيئة كائن UserManager.
            _userStore = userStore; // تهيئة كائن UserStore.
            _emailStore = GetEmailStore(); // استدعاء وظيفة استرجاع EmailStore.
            _signInManager = signInManager; // تهيئة كائن SignInManager.
            _logger = logger; // تهيئة كائن Logger.
            _emailSender = emailSender; // تهيئة كائن EmailSender.
        }

        // النموذج الذي يحتوي على بيانات الإدخال التي يدخلها المستخدم.
        [BindProperty]
        public InputModel Input { get; set; }

        // رابط إعادة التوجيه بعد التسجيل.
        public string ReturnUrl { get; set; }

        // قائمة بأنظمة تسجيل الدخول الخارجية (مثل Google أو Facebook).
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        // تعريف الحقول اللازمة لتسجيل المستخدم.
        public class InputModel
        {
            [Required] // الحقل مطلوب.
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "First Name")] // الاسم الأول.
            public string FirstName { get; set; } // تخزين الاسم الأول.

            [Required] // الحقل مطلوب.
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 2)]
            [Display(Name = "Last Name")] // اسم العائلة.
            public string LastName { get; set; } // تخزين اسم العائلة.

            [Required] // الحقل مطلوب.
            [EmailAddress] // التحقق من صحة البريد الإلكتروني.
            [Display(Name = "Email")] // البريد الإلكتروني.
            public string Email { get; set; } // تخزين البريد الإلكتروني.

            [Required] // الحقل مطلوب.
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)] // تعريف الحقل ككلمة مرور.
            [Display(Name = "Password")] // كلمة المرور.
            public string Password { get; set; } // تخزين كلمة المرور.

            [DataType(DataType.Password)] // تعريف الحقل ككلمة مرور.
            [Display(Name = "Confirm password")] // تأكيد كلمة المرور.
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } // تخزين تأكيد كلمة المرور.
        }

        // يتم استدعاؤها عند تحميل صفحة التسجيل.
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl; // تعيين رابط الإعادة إذا تم تحديده.
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(); // جلب خيارات تسجيل الدخول الخارجية.
        }

        // يتم استدعاؤها عند إرسال بيانات التسجيل من قبل المستخدم.
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/"); // تعيين الرابط الافتراضي إذا لم يتم تحديد رابط إعادة.
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList(); // جلب خيارات تسجيل الدخول الخارجية.

            // تعديل الكود ليكون فقط مرة واحدة
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = new MailAddress(Input.Email).User,
                    Email = Input.Email,
                    FirstName = Input.FirstName,
                    LastName = Input.LastName
                };

                // لا حاجة لإنشاء المستخدم مرة أخرى في CreateUser
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _userManager.AddToRoleAsync(user,"User");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return Page();

        }

        // دالة لإنشاء كائن مستخدم جديد.
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>(); // محاولة إنشاء الكائن.
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        // جلب التخزين الخاص بالبريد الإلكتروني.
        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail) // التحقق من دعم البريد الإلكتروني.
            {
                throw new NotSupportedException("The default UI requires a user store with email support."); // إذا لم يكن البريد الإلكتروني مدعومًا.
            }
            return (IUserEmailStore<ApplicationUser>)_userStore; // إرجاع التخزين الخاص بالبريد الإلكتروني.
        }
    }
}
