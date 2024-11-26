using System.ComponentModel.DataAnnotations;

namespace TaskEngAmr.ViewModels
{
    public class AddUserViewModel
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
        [Display(Name = "Username")] // اسم المستخدم.
        [StringLength(100)]
        public string UserName { get; set; } // تخزين اسم المستخدم.

        [Required] // الحقل مطلوب.
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)] // تعريف الحقل ككلمة مرور.
        [Display(Name = "Password")] // كلمة المرور.
        public string Password { get; set; } // تخزين كلمة المرور.

        [DataType(DataType.Password)] // تعريف الحقل ككلمة مرور.
        [Display(Name = "Confirm password")] // تأكيد كلمة المرور.
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } // تخزين تأكيد كلمة المرور.
        public List<RoleViewModel> Roles { get; set; }


    }
}
