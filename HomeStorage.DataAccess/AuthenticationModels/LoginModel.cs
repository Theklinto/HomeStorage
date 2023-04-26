using System.ComponentModel.DataAnnotations;

namespace HomeStorage.DataAccess.AuthenticationModels
{
    public partial class LoginModel
    {
        [Required(ErrorMessage = "Email er påkrævet")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kodeord er påkrævet")]
        public string Password { get; set; } = string.Empty;
    }
}
