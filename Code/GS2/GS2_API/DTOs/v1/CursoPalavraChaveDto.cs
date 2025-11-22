namespace GS2_API.DTOs.v1
{
    public class CursoPalavraChaveDto
    {
        public int CursoId { get; set; }
        public int PalavraChaveId { get; set; }
        public PalavraChaveDto PalavraChave { get; set; }
    }
}
