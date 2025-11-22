namespace GS2_API.DTOs.v1
{
    public class CursoDto
    {
        public int CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int CargaHorariaHrs { get; set; }

        public int NivelId { get; set; }
        public int ProprietarioId { get; set; }
        public int? TrilhaAprendizagemId { get; set; }
        public string NivelNome { get; set; }
        public string ProprietarioNome { get; set; }
        public string TrilhaTitulo { get; set; }

        public ICollection<ConteudoDto>? Conteudos { get; set; }
    }
}
