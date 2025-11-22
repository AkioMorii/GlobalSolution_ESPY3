using GS2_APP.Models;

namespace GS2_APP.Services
{
    public class TrilhaAprendizagemService
    {
        private readonly ApiClientService _apiClientService;
        private const string ENDPOINT = "trilha";

        public TrilhaAprendizagemService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;
        }

        public async Task<IEnumerable<TrilhaAprendizagemModel>> BuscarTodos()
        {
            var response = await _apiClientService.GetAllAsync<List<TrilhaAprendizagemModel>>(ENDPOINT);
            if (response == null || response.Data == null)
                return Enumerable.Empty<TrilhaAprendizagemModel>();

            return response.Data;
        }

        public async Task<TrilhaAprendizagemModel> Buscar(int id)
        {
            var response = await _apiClientService.GetOneAsync<TrilhaAprendizagemModel>(ENDPOINT, id);
            if (response == null || response.Data == null)
                return new TrilhaAprendizagemModel();

            return response.Data;
        }
        public async Task<bool> Deletar(int id)
        {
            return await _apiClientService.DeleteAsync(ENDPOINT, id);
        }
        public async Task<bool> Criar(TrilhaAprendizagemModel model)
        {
            var response = await _apiClientService.PostAsync<TrilhaAprendizagemModel>(ENDPOINT, model);
            return (response == null || response.Data == null) ? false : true;
        }
        public async Task<bool> Editar(TrilhaAprendizagemModel model)
        {
            var response = await _apiClientService.PutAsync<TrilhaAprendizagemModel>(model.Id, ENDPOINT, model);
            return (response == null || response.Data == null) ? false : true;
        }
    }
}
