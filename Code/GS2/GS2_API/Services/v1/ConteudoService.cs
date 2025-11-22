using AutoMapper;
using GS2_Domain.Entities;
using GS2_Infrastructure.Repository;
using GS2_API.DTOs.v1;

namespace GS2_API.Services.v1
{
    public class ConteudoService
    {
        private readonly ConteudoRepository _repo;
        private readonly IMapper _mapper;

        public ConteudoService(ConteudoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<ConteudoDto>> ListarAsync()
        {
            var lista = await _repo.ListarAsync();
            return _mapper.Map<List<ConteudoDto>>(lista);
        }

        public async Task<ConteudoDto?> ObterPorIdAsync(int id)
        {
            var conteudo = await _repo.ObterPorIdAsync(id);
            return conteudo == null ? null : _mapper.Map<ConteudoDto>(conteudo);
        }

        public async Task<ConteudoDto> CriarAsync(ConteudoDto request)
        {
            var conteudo = _mapper.Map<Conteudo>(request);
            conteudo.DataCadastro = DateTime.UtcNow;

            await _repo.CriarAsync(conteudo);
            return _mapper.Map<ConteudoDto>(conteudo);
        }

        public async Task<ConteudoDto?> AtualizarAsync(int id, ConteudoDto request)
        {
            var conteudo = await _repo.ObterPorIdAsync(id);
            if (conteudo == null) return null;

            _mapper.Map(request, conteudo);
            await _repo.AtualizarAsync(conteudo);

            return _mapper.Map<ConteudoDto>(conteudo);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _repo.ExcluirAsync(id);
        }
    }
}
