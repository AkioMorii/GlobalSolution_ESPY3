namespace GS2_Domain.Entities
{
    public class CursoPalavraChave
    {
        public int CursoId { get; set; }
        public int PalavraChaveId { get; set; }

        // Relacionamentos
        public Curso Curso { get; set; }
        public PalavrasChaves PalavraChave { get; set; }
        protected CursoPalavraChave() { }
    }
}
