namespace GS2_Domain.Entities
{
    public class TrilhaAprendizagem
    {
        public int TrilhaAprendizagemId { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public int CargaHoraria { get; set; }

        public ICollection<Curso> Cursos { get; private set; }

        protected TrilhaAprendizagem()
        {
            Cursos = new List<Curso>();
        }
    }
}   
