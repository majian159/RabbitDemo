using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.UserInterface.ViewModels
{
    public sealed class SignInViewModel
    {
        [DisplayName("账号")]
        [Required]
        public string Account { get; set; }

        [DisplayName("密码")]
        [Required]
        public string Password { get; set; }

        public bool Remember { get; set; }

        public string ReturnUrl { get; set; }
    }

    public sealed class RegisterViewModel
    {
        [DisplayName("用户名")]
        [Required, StringLength(20, MinimumLength = 2)]
        public string UserName { get; set; }

        [DisplayName("密码")]
        [Required, StringLength(15, MinimumLength = 6)]
        public string Password { get; set; }

        [DisplayName("确认密码")]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [DisplayName("账号")]
        [Required, StringLength(20)]
        public string Account { get; set; }
    }
}