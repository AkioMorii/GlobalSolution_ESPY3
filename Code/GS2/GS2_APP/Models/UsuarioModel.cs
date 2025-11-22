using System.ComponentModel.DataAnnotations;

namespace GS2_APP.Models
{
    public class UsuarioModel
    {
        [Key]
        public int usuarioId { get; set; }
        public int Id => usuarioId;

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^(\d{11}|\d{3}\.\d{3}\.\d{3}-\d{2})$",
        ErrorMessage = "CPF inválido. Use 00000000000 ou 000.000.000-00.")]
        public string? Cpf { get; set; }
        [Required(ErrorMessage = "Telefone é obrigatório.")]
        [Phone(ErrorMessage = "Telefone inválido.")]
        [StringLength(15, ErrorMessage = "O telefone deve ter no máximo 15 caracteres.")]
        public string? Telefone { get; set; }
        [Required(ErrorMessage = "Login é obrigatório.")]
        [StringLength(150, MinimumLength = 8, ErrorMessage = "Login deve conter no mínimo 8 caracteres.")]
        public string Login { get; set; } = null!;
        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).+$",
        ErrorMessage = "A senha deve conter letra maiúscula, minúscula, número e caractere especial.")]
        public string? Senha { get; set; }
        [Required(ErrorMessage = "A confirmação é obrigatória.")]
        [Compare("Senha", ErrorMessage = "A senha e a confirmação não coincidem.")]
        public string? ConfirmarSenha { get; set; }
        [Required(ErrorMessage = "O e-mail é obrigatório.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        [Required(ErrorMessage = "O Perfil é obrigatório.")]
        public int TipoUsuarioId { get; set; }
        public string TipoUsuarioDescricao { get; set; } = string.Empty;
        public List<object> CursosCriados { get; set; } = new();
        public List<object> MeusCursos { get; set; } = new();
        public string AtivoDescricao => Ativo ? "Ativo" : "Inativo";

    }
}
