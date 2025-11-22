namespace GS2_Domain.Entities
{
    public class Curso
    {
        public int CursoId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int CargaHorariaHrs { get; set; }
        public int NivelId { get; set; }
        public int ProprietarioId { get; set; }
        public int? TrilhaAprendizagemId { get; set; }

        public Nivel Nivel { get; set; }
        public Usuario Proprietario { get; set; }
        public TrilhaAprendizagem TrilhaAprendizagem { get; set; }
        public ICollection<MeusCursos> MeusCursos { get; set; }
        public ICollection<CursoPalavraChave> CursosPalavrasChave { get; set; }
        public ICollection<Conteudo> Conteudos { get; set; }
        protected Curso()
        {
            MeusCursos = new List<MeusCursos>();
            CursosPalavrasChave = new List<CursoPalavraChave>();
            Conteudos = new List<Conteudo>();
        }
    }
}
