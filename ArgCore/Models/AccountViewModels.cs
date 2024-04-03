using System.ComponentModel.DataAnnotations;

namespace ArgCore.Models
{
    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class TFAViewModel
    {
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [MaxLength(6, ErrorMessage = "Incorrect Code")]
        [RegularExpression("([0-9]+)", ErrorMessage = "Please enter valid code")]
        public string TFACode { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string UserUniqueKey { get; set; }
        public string BarCodeImageUrl { get; set; }
        public string SetupCode { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Client Code")]
        public string clientcode { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        //[Required]
        //[DataType(DataType.Text)]
        //[Display(Name = "User Name")]
        //public string UserName { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        //[Required]
        //[Display(Name = "First Name")]
        //public string firstname { get; set; }

        //[Required]
        //[Display(Name = "Last Name")]
        //public string lastname { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string title { get; set; }

        [RegularExpression("(?=.*[0-9])(?=.*[!@#$%&])[0-9a-zA-Z!@#$%^&0-9]{10,}$", ErrorMessage = "Passwords must have at least one non letter or digit character.<br />Passwords must have at least one digit ('0'-'9').<br />Passwords must have at least one lowercase ('A'-'Z'). <br />Passwords must have at least one uppercase ('A'-'Z').")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [RegularExpression("(?=.*[0-9])(?=.*[!@#$%&])[0-9a-zA-Z!@#$%^&0-9]{10,}$", ErrorMessage = "Passwords must have at least one non letter or digit character.<br />Passwords must have at least one digit ('0'-'9').<br />Passwords must have at least one lowercase ('A'-'Z'). <br />Passwords must have at least one uppercase ('A'-'Z').")]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 10)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
