namespace GS2_Domain.Entities
{
    public class Conteudo
    {
        public int ConteudoId { get; set; }
        public int CursoId { get; set; }
        public string Url { get; set; }
        public DateTime DataCadastro { get; set; }
        public int Ordem { get; set; }
        public int TipoConteudoId { get; set; }

        // Relacionamentos (opcionais para EF Core)
        public Curso Curso { get; set; }
        public TipoConteudo TipoConteudo { get; set; }
        protected Conteudo() { }
    }
}
