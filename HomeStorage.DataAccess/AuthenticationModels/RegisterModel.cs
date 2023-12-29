using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeStorage.DataAccess.AuthenticationModels
{
    public partial class RegisterModel
    {
        [Required(ErrorMessage = "Brugernavn er påkrævet")]
        public string Username { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Email er påkrævet")]
        public string Email { get; set; } = string.Empty;

        [PasswordPropertyText]
        [Required(ErrorMessage = "Kodeord er påkrævet")]
        public string Password { get; set; } = string.Empty;
    }
}
