namespace GS2_Domain.Entities
{
    public class TipoConteudo
    {
        public int TipoConteudoId { get; private set; }
        public string Descricao { get; private set; }
        public ICollection<Conteudo> Conteudos { get; private set; }
        protected TipoConteudo() { Conteudos = new List<Conteudo>(); }
    }
}
