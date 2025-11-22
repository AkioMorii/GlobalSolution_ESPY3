namespace GS2_Domain.Entities
{
    public class Nivel
    {
        public int NivelId { get; private set; }
        public string Descricao { get; private set; }
        public ICollection<Curso> Cursos { get; private set; }
        protected Nivel() { Cursos = new List<Curso>(); }
    }
}
