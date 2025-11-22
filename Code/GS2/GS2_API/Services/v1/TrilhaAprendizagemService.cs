using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_API.Services.v1
{
    public class TrilhaAprendizagemService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TrilhaAprendizagemService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TrilhaAprendizagemDto> CriarAsync(TrilhaAprendizagemDto dto)
        {
            var entity = _mapper.Map<TrilhaAprendizagem>(dto);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<TrilhaAprendizagemDto>(entity);
        }

        public async Task<TrilhaAprendizagemDto> ObterPorIdAsync(int id)
        {
            var entity = await _context.Set<TrilhaAprendizagem>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Trilha Aprendizagem com ID {id} não encontrada.");

            return _mapper.Map<TrilhaAprendizagemDto>(entity);
        }

        public async Task<List<TrilhaAprendizagemDto>> BuscarAsync(string? descricao)
        {
            var query = _context.Set<TrilhaAprendizagem>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(descricao))
            {
                query = query.Where(p => p.Descricao.Contains(descricao));
            }

            var list = await query.ToListAsync();
            return _mapper.Map<List<TrilhaAprendizagemDto>>(list);
        }

        public async Task<TrilhaAprendizagemDto> AtualizarAsync(int id, TrilhaAprendizagemDto dto)
        {
            var entity = await _context.Set<TrilhaAprendizagem>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Trilha Aprendizagem com ID {id} não encontrada.");

            entity.GetType().GetProperty(nameof(TrilhaAprendizagem.Descricao))!
                  .SetValue(entity, dto.Descricao);

            await _context.SaveChangesAsync();
            return _mapper.Map<TrilhaAprendizagemDto>(entity);
        }

        public async Task ExcluirAsync(int id)
        {
            var entity = await _context.Set<TrilhaAprendizagem>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Trilha Aprendizagem com ID {id} não encontrada.");

            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
