using GS2_APP.Models;

namespace GS2_APP.Services
{
    public class UsuarioService
    {
        private readonly ApiClientService _apiClientService;
        private const string ENDPOINT = "usuario";
        public UsuarioService(ApiClientService apiClientService)
        {
            _apiClientService = apiClientService;

        }
        public async Task<IEnumerable<UsuarioModel>> Buscar()
        {
            var response = await _apiClientService
                .GetAllAsync<List<UsuarioModel>>(ENDPOINT);

            if (response == null || response.Data == null)
                return Enumerable.Empty<UsuarioModel>();

            return response.Data;
        }
        public async Task<UsuarioModel> Buscar(int id) {
            var response = await _apiClientService.GetOneAsync<UsuarioModel>(ENDPOINT, id);
            if (response == null || response.Data == null)
                return new UsuarioModel();

            return response.Data;
        }
        public async Task<bool> Deletar(int id) { 
            return await _apiClientService.DeleteAsync(ENDPOINT, id);
        }
        public async Task<bool> Criar(UsuarioModel model)
        {
            var response = await _apiClientService.PostAsync<UsuarioModel>(ENDPOINT, model);
            return (response == null || response.Data == null)?false:true;
        }
        public async Task<bool> Editar(UsuarioModel model)
        {
            var response = await _apiClientService.PutAsync<UsuarioModel>(model.Id,ENDPOINT, model);
            return (response == null || response.Data == null) ? false : true;
        }
    }
}
