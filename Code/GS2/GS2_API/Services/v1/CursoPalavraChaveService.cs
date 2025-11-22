using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;
using GS2_Infrastructure.Repository;

namespace GS2_API.Services.v1
{
    public class CursoPalavraChaveService
    {
        private readonly CursoPalavraChaveRepository _repo;
        private readonly IMapper _mapper;

        public CursoPalavraChaveService(CursoPalavraChaveRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CursoPalavraChaveDto>> ListarPorCursoAsync(int cursoId)
        {
            var entities = await _repo.ListarPorCursoAsync(cursoId);
            return _mapper.Map<List<CursoPalavraChaveDto>>(entities);
        }

        public async Task<CursoPalavraChaveDto> AdicionarAsync(CursoPalavraChaveDto dto)
        {
            var entity = _mapper.Map<CursoPalavraChave>(dto);
            await _repo.AdicionarAsync(entity);
            return _mapper.Map<CursoPalavraChaveDto>(entity);
        }

        public async Task RemoverAsync(int cursoId, int palavraChaveId)
        {
            await _repo.RemoverAsync(cursoId, palavraChaveId);
        }
    }
}
