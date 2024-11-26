using System.ComponentModel.DataAnnotations;

namespace TaskEngAmr.ViewModels
{
    public class RoleFormViewModel
    {
        [Required, MaxLength(256)]
        public string Name { get; set; }
    }
}
