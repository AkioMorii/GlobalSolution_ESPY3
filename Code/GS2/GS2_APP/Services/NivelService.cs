using GS2_APP.Models;

namespace GS2_APP.Services
{
    public class NivelService
    {
        private readonly ApiClientService _apiClientService;
        private const string ENDPOINT = "nivel";

        public NivelService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IEnumerable<NivelModel>> BuscarTodos()
        {
            var response = await _apiClientService.GetAllAsync<List<NivelModel>>(ENDPOINT);
            if (response == null || response.Data == null)
                return Enumerable.Empty<NivelModel>();

            return response.Data;
        }

        public async Task<NivelModel> Buscar(int id)
        {
            var response = await _apiClientService.GetOneAsync<NivelModel>(ENDPOINT, id);
            if (response == null || response.Data == null)
                return new NivelModel();

            return response.Data;
        }
    }
}
