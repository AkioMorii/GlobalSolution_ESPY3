using GS2_Domain.Entities;

namespace GS2_Domain.Entities
{
    public class TipoUsuario
    {
        public int TipoUsuarioId { get; set; }
        public string Descricao { get; set; }

        public ICollection<Usuario>? Usuarios { get; private set; }

        protected TipoUsuario()
        {
            Usuarios = new List<Usuario>();
        }
    }
}
