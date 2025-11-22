namespace GS2_APP.Models
{
    public class ConteudoModel
    {
        public int? ConteudoId { get; set; } // null para novos arquivos
        public string Url { get; set; } // preenchido após upload
        public string NomeArquivo { get; set; }
        public IFormFile Arquivo { get; set; } // usado apenas para novos uploads
        public int TipoConteudoId { get; set; }
        public int Ordem { get; set; }
        public string TipoConteudoNome { get; set; } // apenas para exibir
    }
}
