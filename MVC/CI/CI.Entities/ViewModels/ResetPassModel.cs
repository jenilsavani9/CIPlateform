using System.ComponentModel.DataAnnotations;

namespace CI.Entities.ViewModels
{
    public class ResetPassModel
    {
        public string? Email { get; set; }

        public string? Token { get; set; }

        [StringLength(20, MinimumLength = 8, ErrorMessage = "Password length must be between 8 and 20")]
        public string? Password { get; set; }

        public string? ConfirmPassword { get; set; }
    }
}
