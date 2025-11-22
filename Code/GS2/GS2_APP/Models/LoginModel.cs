using System.ComponentModel.DataAnnotations;

namespace GS2_APP.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Login é obrigatório.")]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "Login deve conter no mínimo 8 caracteres.")]
        public string Login { get; set; } = null!;
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "A senha deve conter letra maiúscula, minúscula, número e caractere especial.")]
        public string Senha { get; set; } = null!;
    }
}
