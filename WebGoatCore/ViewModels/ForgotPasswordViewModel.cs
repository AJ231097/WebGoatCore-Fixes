using System.ComponentModel.DataAnnotations;

namespace WebGoatCore.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please enter your username")]
        public string Username { get; set; }
    }
}
