using GS2_APP.Models;

namespace GS2_APP.Services
{
    public class PalavraChaveService
    {
        private readonly ApiClientService _apiClientService;
        private const string ENDPOINT = "palavrachave";

        public PalavraChaveService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IEnumerable<PalavraChaveModel>> BuscarTodos()
        {
            var response = await _apiClientService.GetAllAsync<List<PalavraChaveModel>>(ENDPOINT);
            if (response == null || response.Data == null)
                return Enumerable.Empty<PalavraChaveModel>();

            return response.Data;
        }

        public async Task<PalavraChaveModel> Buscar(int id)
        {
            var response = await _apiClientService.GetOneAsync<PalavraChaveModel>(ENDPOINT, id);
            if (response == null || response.Data == null)
                return new PalavraChaveModel();

            return response.Data;
        }
    }
}
