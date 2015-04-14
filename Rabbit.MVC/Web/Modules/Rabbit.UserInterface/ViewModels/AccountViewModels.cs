using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Rabbit.UserInterface.ViewModels
{
    public class SignInViewModel
    {
        [DisplayName("用户名")]
        [Required]
        public string UserName { get; set; }

        [DisplayName("密码")]
        [Required]
        public string Password { get; set; }

        public bool Remember { get; set; }

        public string ReturnUrl { get; set; }
    }
}