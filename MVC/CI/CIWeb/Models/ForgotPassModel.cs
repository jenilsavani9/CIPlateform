using System.ComponentModel.DataAnnotations;

namespace CIWeb.Models
{
    public class ForgotPassModel
    {
        [Required(ErrorMessage = "please enter Email")]
        public string? Email { get; set; }
    }
}
