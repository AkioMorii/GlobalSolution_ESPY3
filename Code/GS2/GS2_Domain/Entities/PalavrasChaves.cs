namespace GS2_Domain.Entities
{
    public class PalavrasChaves
    {
        public int PalavraChaveId { get; private set; }
        public string Descricao { get; private set; }
        public ICollection<CursoPalavraChave> CursosPalavrasChave { get; set; }
        protected PalavrasChaves()
        {
            CursosPalavrasChave = new List<CursoPalavraChave>();
        }
    }
}
