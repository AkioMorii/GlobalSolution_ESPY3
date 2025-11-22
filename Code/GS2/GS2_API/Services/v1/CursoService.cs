using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;
using GS2_Infrastructure.Repository;

namespace GS2_API.Services.v1
{
    public class CursoService
    {
        private readonly CursoRepository _repo;
        private readonly IMapper _mapper;

        public CursoService(CursoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<CursoDto>> ListarAsync()
        {
            var cursos = await _repo.ListarAsync();
            return _mapper.Map<List<CursoDto>>(cursos);
        }

        public async Task<CursoDto?> ObterPorIdAsync(int id)
        {
            var curso = await _repo.ObterPorIdAsync(id);
            return curso == null ? null : _mapper.Map<CursoDto>(curso);
        }

        public async Task<CursoDto> CriarAsync(CursoDto request)
        {
            try
            {
                var curso = _mapper.Map<Curso>(request);

                await _repo.CriarAsync(curso);
                return _mapper.Map<CursoDto>(curso);
            }
            catch (Exception ex) {
                var error = ex.Message;
            
            }
            return default;
            
        }

        public async Task<CursoDto?> AtualizarAsync(int id, CursoDto request)
        {
            var cursoDb = await _repo.ObterPorIdAsync(id);
            if (cursoDb == null) return null;

            _mapper.Map(request, cursoDb);

            await _repo.AtualizarAsync(cursoDb);

            return _mapper.Map<CursoDto>(cursoDb);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            return await _repo.ExcluirAsync(id);
        }
    }
}
