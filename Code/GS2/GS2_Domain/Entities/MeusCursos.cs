namespace GS2_Domain.Entities
{
    public class MeusCursos
    {
        public int UsuarioId { get; set; }
        public int CursoId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public bool Ativo { get; set; }

        public Usuario Usuario { get; set; }
        public Curso Curso { get; set; }
        protected MeusCursos() { }
    }
}
