using System.ComponentModel.DataAnnotations;

namespace Ait.Auth.Api.Models
{
    public class UserModel
    {
        [Required]
        [Display(Name = "Пользователь")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "В пароле должно быть по крайней мере {2} символов.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Повторить пароль")]
        [Compare("Password", ErrorMessage = "Пароль и подтверждение пароля не совпадают.")]
        public string ConfirmPassword { get; set; }
    }
}