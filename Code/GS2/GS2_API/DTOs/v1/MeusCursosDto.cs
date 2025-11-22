namespace GS2_API.DTOs.v1
{
    public class MeusCursosDto
    {
        public int CursoId { get; set; }
        public string NomeCurso { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string CursoTitulo { get; set; }
    }
    public class MeusCursosCreateDto
    {
        public int UsuarioId { get; set; }
        public int CursoId { get; set; }
    }
    public class MeusCursosConclusaoDto
    {
        public int UsuarioId { get; set; }
        public int CursoId { get; set; }
    }
}
