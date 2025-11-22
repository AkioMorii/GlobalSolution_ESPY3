using System.ComponentModel.DataAnnotations;

namespace GS2_APP.Models
{
    public class TrilhaAprendizagemModel
    {
        [Key]
        public int TrilhaAprendizagemId { get; set; }
        public int Id => TrilhaAprendizagemId;
        [Required(ErrorMessage = "O Titulo é obrigatório.")]
        [StringLength(100, ErrorMessage = "O Titulo deve ter no máximo 100 caracteres.")]
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int  CargaHoraria { get; set; }
        public DateOnly DataCadastro { get; set; }
    }
}
