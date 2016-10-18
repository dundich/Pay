using System.ComponentModel.DataAnnotations;

namespace Ait.Auth.Api.Models
{
    public class ChangePasswordModel
    {
        [Required]
        [Display(Name = "Пользователь")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Пароль должен быть по крайней мере {2} символов.", MinimumLength = ShellConsts.PasswordMinimumLength)]
        [DataType(DataType.Password)]
        [Display(Name = "Старый пароль")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Новый пароль")]
        [StringLength(100, ErrorMessage = "Новый пароль должен быть по крайней мере {2} символов.", MinimumLength = ShellConsts.PasswordMinimumLength)]
        public string NewPassword { get; set; }
    }
}