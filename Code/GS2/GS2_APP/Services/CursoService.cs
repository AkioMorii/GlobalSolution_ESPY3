using GS2_APP.Models;
using System.Reflection;

namespace GS2_APP.Services
{
    public class CursoService
    {
        private readonly ApiClientService _apiClientService;
        private const string ENDPOINT = "curso";

        public CursoService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        // ------------------- LISTAGEM -------------------
        public async Task<IEnumerable<CursoModel>> BuscarTodos()
        {
            var response = await _apiClientService.GetAllAsync<List<CursoModel>>(ENDPOINT);
            if (response == null || response.Data == null)
                return Enumerable.Empty<CursoModel>();

            return response.Data;
        }

        // ------------------- DETALHES -------------------
        public async Task<CursoModel> Buscar(int id)
        {
            var response = await _apiClientService.GetOneAsync<CursoModel>(ENDPOINT, id);
            return response?.Data ?? new CursoModel();
        }

        // ------------------- DELETAR -------------------
        public async Task<bool> Deletar(int id)
        {
            return await _apiClientService.DeleteAsync(ENDPOINT, id);
        }

        // ------------------- CRIAR -------------------
        public async Task<bool> Criar(CursoModel model, IEnumerable<IFormFile>? arquivos = null)
        {
            await ProcessarConteudos(model, arquivos);
            var response = await _apiClientService.PostAsync<CursoModel>(ENDPOINT, model);
            return response?.Data != null;
        }

        // ------------------- EDITAR -------------------
        public async Task<bool> Editar(CursoModel model, IEnumerable<IFormFile>? arquivos = null)
        {
            await ProcessarConteudos(model, arquivos);
            var response = await _apiClientService.PutAsync<CursoModel>(model.Id, ENDPOINT, model);
            return response?.Data != null;
        }

        // ------------------- AUXILIARES -------------------

        private async Task ProcessarConteudos(CursoModel model, IEnumerable<IFormFile>? arquivos)
        {
            // 1. Associa arquivos enviados aos conteúdos
            if (arquivos != null && arquivos.Any() && model.Conteudos != null)
            {
                foreach (var file in arquivos)
                {
                    var conteudo = model.Conteudos.FirstOrDefault(c => c.NomeArquivo == file.FileName);
                    if (conteudo != null)
                        conteudo.Arquivo = file;
                }
            }

            // 2. Envia para API os arquivos que possuem IFormFile
            if (model.Conteudos != null && model.Conteudos.Any(c => c.Arquivo != null))
            {
                var uploaded = await UploadArquivos(model.Conteudos);
                // Atualiza URLs retornadas pela API
                foreach (var u in uploaded)
                {
                    var c = model.Conteudos.FirstOrDefault(x => x.NomeArquivo == u.NomeArquivo);
                    if (c != null)
                        c.Url = u.Url;
                }
            }

            // 3. Inclui nova palavra-chave, se houver
            if (!string.IsNullOrWhiteSpace(model.NovaPalavraChave))
            {
                model.PalavrasChaveSelecionadas.Add(0); // 0 indica criação na API
            }
        }

        private async Task<List<ConteudoModel>> UploadArquivos(List<ConteudoModel> conteudos)
        {
            if (conteudos == null || !conteudos.Any(c => c.Arquivo != null))
                return new List<ConteudoModel>();

            using var form = new MultipartFormDataContent();

            foreach (var c in conteudos.Where(x => x.Arquivo != null))
            {
                var fileContent = new StreamContent(c.Arquivo.OpenReadStream());
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(c.Arquivo.ContentType);
                form.Add(fileContent, "arquivos", c.NomeArquivo);

                // Adiciona metadados
                form.Add(new StringContent(c.TipoConteudoId.ToString()), "TipoConteudoId");
                form.Add(new StringContent(c.Ordem.ToString()), "Ordem");
            }

            // Aqui usamos a lista que recebemos como parâmetro
            var response = await _apiClientService.PostFilesAsync<List<ConteudoModel>>("conteudo/upload", conteudos);

            return response?.Data ?? new List<ConteudoModel>();
        }
    }
}
