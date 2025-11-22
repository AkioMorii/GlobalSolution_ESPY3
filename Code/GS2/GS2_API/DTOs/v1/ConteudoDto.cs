namespace GS2_API.DTOs.v1
{
    public class ConteudoDto
    {
        public int ConteudoId { get; set; }
        public int CursoId { get; set; }
        public string NomeArquivo { get; set; }
        public string Url { get; set; }
        public DateTime DataCadastro { get; set; }
        public int Ordem { get; set; }
        public TipoConteudoDto TipoConteudo { get; set; }
    }
}
