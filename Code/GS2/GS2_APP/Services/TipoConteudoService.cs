using GS2_APP.Models;

namespace GS2_APP.Services
{
    public class TipoConteudoService
    {
        private readonly ApiClientService _apiClientService;
        private const string ENDPOINT = "tipoconteudo";

        public TipoConteudoService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IEnumerable<TipoConteudoModel>> BuscarTodos()
        {
            var response = await _apiClientService.GetAllAsync<List<TipoConteudoModel>>(ENDPOINT);
            if (response == null || response.Data == null)
                return Enumerable.Empty<TipoConteudoModel>();

            return response.Data;
        }

        public async Task<TipoConteudoModel> Buscar(int id)
        {
            var response = await _apiClientService.GetOneAsync<TipoConteudoModel>(ENDPOINT, id);
            if (response == null || response.Data == null)
                return new TipoConteudoModel();

            return response.Data;
        }
    }
}
