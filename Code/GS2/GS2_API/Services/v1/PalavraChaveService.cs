using AutoMapper;
using GS2_API.DTOs.v1;
using GS2_Domain.Entities;
using GS2_Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace GS2_API.Services.v1
{
    public class PalavraChaveService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PalavraChaveService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PalavraChaveDto> CriarAsync(PalavraChaveDto dto)
        {
            var entity = _mapper.Map<PalavrasChaves>(dto);
            _context.Add(entity);
            await _context.SaveChangesAsync();
            return _mapper.Map<PalavraChaveDto>(entity);
        }

        public async Task<PalavraChaveDto> ObterPorIdAsync(int id)
        {
            var entity = await _context.Set<PalavrasChaves>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Palavra-chave com ID {id} não encontrada.");

            return _mapper.Map<PalavraChaveDto>(entity);
        }

        public async Task<List<PalavraChaveDto>> BuscarAsync(string? descricao)
        {
            var query = _context.Set<PalavrasChaves>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(descricao))
            {
                query = query.Where(p => p.Descricao.Contains(descricao));
            }

            var list = await query.ToListAsync();
            return _mapper.Map<List<PalavraChaveDto>>(list);
        }

        public async Task<PalavraChaveDto> AtualizarAsync(int id, PalavraChaveDto dto)
        {
            var entity = await _context.Set<PalavrasChaves>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Palavra-chave com ID {id} não encontrada.");

            entity.GetType().GetProperty(nameof(PalavrasChaves.Descricao))!
                  .SetValue(entity, dto.Descricao);

            await _context.SaveChangesAsync();
            return _mapper.Map<PalavraChaveDto>(entity);
        }

        public async Task ExcluirAsync(int id)
        {
            var entity = await _context.Set<PalavrasChaves>().FindAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Palavra-chave com ID {id} não encontrada.");

            _context.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
