using System.ComponentModel.DataAnnotations;

namespace GS2_APP.Models
{
    public class CursoModel
    {
        [Key]
        public int CursoId { get; set; }
        public int Id => CursoId;

        // --------- Dados principais ----------
        [Required(ErrorMessage = "O título é obrigatório.")]
        [StringLength(200, ErrorMessage = "O título deve ter no máximo 200 caracteres.")]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(2000, ErrorMessage = "A descrição deve ter no máximo 2000 caracteres.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "A carga horária é obrigatória.")]
        [Range(1, 1000, ErrorMessage = "A carga horária deve ser maior que zero.")]
        public int CargaHorariaHrs { get; set; }

        // --------- Nível ----------
        [Required(ErrorMessage = "O nível é obrigatório.")]
        public int NivelId { get; set; }
        public string NivelNome { get; set; } = string.Empty;

        // --------- Proprietário (Instrutor logado) ----------
        [Required(ErrorMessage = "O proprietário é obrigatório.")]
        public int ProprietarioId { get; set; }
        public string ProprietarioNome { get; set; } = string.Empty;

        public int? TrilhaAprendizagemId { get; set; }
        public string? TrilhaTitulo { get; set; } = string.Empty;

        public List<int> PalavrasChaveSelecionadas { get; set; } = new();

        public string? NovaPalavraChave { get; set; }

        public List<ConteudoModel> Conteudos { get; set; } = new();

        // --------- Usado para exibir selects na View ----------
        public List<NivelModel> ListaNiveis { get; set; } = new();
        public List<TrilhaAprendizagemModel> ListaTrilhas { get; set; } = new();
        public List<PalavraChaveModel> ListaPalavrasChave { get; set; } = new();
        public List<TipoConteudoModel>ListaTiposConteudos { get; set; } = new();
    }

}
