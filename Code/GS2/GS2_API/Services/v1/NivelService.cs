using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_API.Services.v1
{
    public class NivelService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public NivelService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<NivelDto>> ListarAsync()
        {
            var lista = await _context.Niveis
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<List<NivelDto>>(lista);
        }

        public async Task<NivelDto?> ObterPorIdAsync(int id)
        {
            var nivel = await _context.Niveis
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.NivelId == id);

            return nivel == null ? null : _mapper.Map<NivelDto>(nivel);
        }

        public async Task<NivelDto> CriarAsync(NivelDto dto)
        {
            var nivel = _mapper.Map<Nivel>(dto);

            _context.Niveis.Add(nivel);
            await _context.SaveChangesAsync();

            return _mapper.Map<NivelDto>(nivel);
        }

        public async Task<NivelDto?> AtualizarAsync(int id, NivelDto dto)
        {
            var nivel = await _context.Niveis.FirstOrDefaultAsync(x => x.NivelId == id);
            if (nivel == null) return null;

            _mapper.Map(dto, nivel);

            await _context.SaveChangesAsync();
            return _mapper.Map<NivelDto>(nivel);
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            var nivel = await _context.Niveis.FirstOrDefaultAsync(x => x.NivelId == id);
            if (nivel == null) return false;

            _context.Niveis.Remove(nivel);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
